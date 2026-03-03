import React, { useCallback, useEffect, useRef, useState } from 'react';
import { LEVELS, CAT_SPEED, ITEM_SIZE, FREEZE_DURATION, MAGNET_DURATION, STAR_BOOST_DURATION, ROBO_BOT_INTERVAL_MIN, ROBO_BOT_INTERVAL_MAX, FRUIT_TYPES } from '@/game/constants';
import { FallingItem, GameState, ItemType, RoboBot } from '@/game/types';
import AstroCat from './AstroCat';
import FruitSVG from './FruitSVG';
import RoboBotSVG from './RoboBotSVG';
import { Pause } from 'lucide-react';

interface GameScreenProps {
  levelId: number;
  onWin: (score: number) => void;
  onPause: () => void;
  onMenu: () => void;
}

const GameScreen: React.FC<GameScreenProps> = ({ levelId, onWin, onPause, onMenu }) => {
  const level = LEVELS[levelId - 1];
  const containerRef = useRef<HTMLDivElement>(null);
  const [countdown, setCountdown] = useState(3);
  const [playing, setPlaying] = useState(false);
  const [paused, setPaused] = useState(false);
  const gameRef = useRef<GameState>({
    score: 0, progress: 0, catX: 0, frozen: false, frozenTimer: 0,
    magnetActive: false, magnetTimer: 0, starBoost: false, starBoostTimer: 0,
    items: [], roboBot: null, roboBotTimer: 0, spawnTimer: 0, nextItemId: 0,
  });
  const keysRef = useRef<Set<string>>(new Set());
  const animRef = useRef<number>(0);
  const [renderTick, setRenderTick] = useState(0);
  const [containerSize, setContainerSize] = useState({ w: 800, h: 600 });
  const [touchSide, setTouchSide] = useState<'left' | 'right' | null>(null);
  const [showCatchEffect, setShowCatchEffect] = useState<{ x: number; y: number; type: string } | null>(null);

  // Container size
  useEffect(() => {
    const updateSize = () => {
      if (containerRef.current) {
        setContainerSize({ w: containerRef.current.clientWidth, h: containerRef.current.clientHeight });
      }
    };
    updateSize();
    window.addEventListener('resize', updateSize);
    return () => window.removeEventListener('resize', updateSize);
  }, []);

  // Initialize cat position
  useEffect(() => {
    gameRef.current.catX = containerSize.w / 2;
    gameRef.current.roboBotTimer = ROBO_BOT_INTERVAL_MIN + Math.random() * (ROBO_BOT_INTERVAL_MAX - ROBO_BOT_INTERVAL_MIN);
  }, [containerSize.w]);

  // Countdown
  useEffect(() => {
    if (countdown <= 0) {
      setPlaying(true);
      return;
    }
    const t = setTimeout(() => setCountdown(countdown - 1), 800);
    return () => clearTimeout(t);
  }, [countdown]);

  // Keyboard
  useEffect(() => {
    const onDown = (e: KeyboardEvent) => {
      keysRef.current.add(e.key.toLowerCase());
      if (e.key === 'Escape') {
        setPaused(p => !p);
      }
    };
    const onUp = (e: KeyboardEvent) => keysRef.current.delete(e.key.toLowerCase());
    window.addEventListener('keydown', onDown);
    window.addEventListener('keyup', onUp);
    return () => { window.removeEventListener('keydown', onDown); window.removeEventListener('keyup', onUp); };
  }, []);

  // Touch controls
  const handleTouchStart = useCallback((e: React.TouchEvent) => {
    const x = e.touches[0].clientX;
    setTouchSide(x < containerSize.w / 2 ? 'left' : 'right');
  }, [containerSize.w]);
  const handleTouchEnd = useCallback(() => setTouchSide(null), []);

  // Spawn item
  const spawnItem = useCallback(() => {
    const g = gameRef.current;
    const w = containerSize.w;
    let type: ItemType;
    const rand = Math.random();
    if (level.hasSlime && rand < level.slimeChance) {
      type = 'slime';
    } else {
      type = FRUIT_TYPES[Math.floor(Math.random() * FRUIT_TYPES.length)];
    }
    const item: FallingItem = {
      id: g.nextItemId++,
      type,
      x: Math.random() * (w - ITEM_SIZE * 2) + ITEM_SIZE,
      y: -ITEM_SIZE,
      speed: level.fruitSpeed * (0.8 + Math.random() * 0.4),
      swayOffset: level.hasSway ? Math.random() * Math.PI * 2 : undefined,
      swaySpeed: level.hasSway ? 0.02 + Math.random() * 0.02 : undefined,
      opacity: level.hasFade ? 0 : 1,
    };
    g.items.push(item);
  }, [containerSize.w, level]);

  // Game loop
  useEffect(() => {
    if (!playing || paused) return;
    let frame = 0;

    const loop = () => {
      const g = gameRef.current;
      const w = containerSize.w;
      const h = containerSize.h;
      frame++;

      // Move cat
      if (!g.frozen) {
        const speed = CAT_SPEED * (w / 800);
        if (keysRef.current.has('arrowleft') || keysRef.current.has('a') || touchSide === 'left') {
          g.catX = Math.max(30, g.catX - speed);
        }
        if (keysRef.current.has('arrowright') || keysRef.current.has('d') || touchSide === 'right') {
          g.catX = Math.min(w - 30, g.catX + speed);
        }
      }

      // Frozen timer
      if (g.frozen) {
        g.frozenTimer--;
        if (g.frozenTimer <= 0) g.frozen = false;
      }

      // Magnet timer
      if (g.magnetActive) {
        g.magnetTimer--;
        if (g.magnetTimer <= 0) g.magnetActive = false;
      }

      // Star boost timer
      if (g.starBoost) {
        g.starBoostTimer--;
        if (g.starBoostTimer <= 0) g.starBoost = false;
      }

      // Spawn items
      g.spawnTimer--;
      if (g.spawnTimer <= 0) {
        spawnItem();
        g.spawnTimer = level.spawnRate / level.itemDensity;
      }

      // Robo-bot
      if (level.hasRoboBot) {
        g.roboBotTimer--;
        if (g.roboBotTimer <= 0 && !g.roboBot) {
          const dir = Math.random() > 0.5 ? 1 : -1;
          g.roboBot = { x: dir === 1 ? -60 : w + 60, direction: dir as 1 | -1, active: true, droppedPowerUp: false };
        }
        if (g.roboBot?.active) {
          g.roboBot.x += g.roboBot.direction * 3;
          // Drop power-up at middle
          if (!g.roboBot.droppedPowerUp && Math.abs(g.roboBot.x - w / 2) < 30) {
            g.roboBot.droppedPowerUp = true;
            const pType: ItemType = Math.random() > 0.5 ? 'magnet' : 'golden-star';
            g.items.push({
              id: g.nextItemId++, type: pType,
              x: g.roboBot.x, y: 50, speed: level.fruitSpeed * 0.7, opacity: 1,
            });
          }
          if (g.roboBot.x < -80 || g.roboBot.x > w + 80) {
            g.roboBot = null;
            g.roboBotTimer = ROBO_BOT_INTERVAL_MIN + Math.random() * (ROBO_BOT_INTERVAL_MAX - ROBO_BOT_INTERVAL_MIN);
          }
        }
      }

      // Update items
      const catLeft = g.catX - 30;
      const catRight = g.catX + 30;
      const catTop = h - 80;

      g.items = g.items.filter(item => {
        // Sway
        if (item.swayOffset !== undefined && item.swaySpeed !== undefined) {
          item.swayOffset += item.swaySpeed;
          item.x += Math.sin(item.swayOffset) * 1.5;
        }
        // Fade
        if (level.hasFade) {
          item.opacity = 0.3 + Math.abs(Math.sin(frame * 0.02 + item.id)) * 0.7;
        }
        // Magnet
        if (g.magnetActive && item.type !== 'slime') {
          const dx = g.catX - item.x;
          item.x += dx * 0.08;
        }
        item.y += item.speed;

        // Collision
        const itemCX = item.x;
        const itemCY = item.y;
        if (itemCY > catTop - 10 && itemCY < h - 10 && itemCX > catLeft && itemCX < catRight) {
          if (item.type === 'slime') {
            g.frozen = true;
            g.frozenTimer = FREEZE_DURATION;
          } else if (item.type === 'magnet') {
            g.magnetActive = true;
            g.magnetTimer = MAGNET_DURATION;
          } else if (item.type === 'golden-star') {
            g.starBoost = true;
            g.starBoostTimer = STAR_BOOST_DURATION;
            g.progress += 0.1;
          } else {
            g.score++;
            const increment = g.starBoost ? 0.06 : 0.03;
            g.progress = Math.min(1, g.progress + (1 / level.fruitsNeeded) + (g.starBoost ? increment : 0));
          }
          setShowCatchEffect({ x: itemCX, y: itemCY, type: item.type });
          setTimeout(() => setShowCatchEffect(null), 300);
          return false;
        }
        // Off screen
        return item.y < h + ITEM_SIZE;
      });

      // Win condition
      if (g.progress >= 1) {
        cancelAnimationFrame(animRef.current);
        onWin(g.score);
        return;
      }

      setRenderTick(t => t + 1);
      animRef.current = requestAnimationFrame(loop);
    };

    animRef.current = requestAnimationFrame(loop);
    return () => cancelAnimationFrame(animRef.current);
  }, [playing, paused, containerSize, level, spawnItem, onWin, touchSide]);

  const g = gameRef.current;

  return (
    <div
      ref={containerRef}
      className="game-container"
      style={{ background: level.background }}
      onTouchStart={handleTouchStart}
      onTouchEnd={handleTouchEnd}
    >
      {/* Countdown */}
      {!playing && (
        <div className="absolute inset-0 flex items-center justify-center z-50">
          <span
            key={countdown}
            className="animate-countdown-pop text-8xl md:text-9xl font-black"
            style={{
              color: '#ffe066',
              textShadow: '0 0 40px rgba(255, 224, 102, 0.8)',
            }}
          >
            {countdown === 0 ? 'GO!' : countdown}
          </span>
        </div>
      )}

      {/* HUD */}
      {playing && (
        <div className="absolute top-0 left-0 right-0 z-40 flex items-center justify-between px-4 py-2">
          {/* Progress bar */}
          <div className="flex items-center gap-2 flex-1 max-w-xs">
            <div
              className="h-5 flex-1 rounded-full overflow-hidden"
              style={{ background: 'rgba(0,0,0,0.4)', border: '2px solid rgba(255,255,255,0.3)' }}
            >
              <div
                className="h-full rounded-full transition-all duration-200"
                style={{
                  width: `${Math.min(100, g.progress * 100)}%`,
                  background: 'linear-gradient(90deg, #27ae60, #2ecc71, #ffe066)',
                }}
              />
            </div>
          </div>

          {/* Score */}
          <div
            className="text-xl md:text-2xl font-bold px-4 py-1 rounded-xl"
            style={{ background: 'rgba(0,0,0,0.4)', color: '#ffe066', border: '1px solid rgba(255,224,102,0.3)' }}
          >
            ⭐ {g.score}
          </div>

          {/* Pause */}
          <button
            onClick={() => setPaused(true)}
            className="ml-2 p-2 rounded-lg transition-all hover:scale-110"
            style={{ background: 'rgba(0,0,0,0.4)', color: 'white', border: '1px solid rgba(255,255,255,0.2)' }}
          >
            <Pause size={20} />
          </button>
        </div>
      )}

      {/* Active power-up indicators */}
      {playing && (g.magnetActive || g.starBoost) && (
        <div className="absolute top-12 left-4 z-40 flex gap-2">
          {g.magnetActive && (
            <div className="px-2 py-1 rounded-lg text-sm font-bold" style={{ background: 'rgba(231,76,60,0.8)', color: 'white' }}>
              🧲 Magnet!
            </div>
          )}
          {g.starBoost && (
            <div className="px-2 py-1 rounded-lg text-sm font-bold" style={{ background: 'rgba(255,215,0,0.8)', color: '#1a1a2e' }}>
              ⭐ Star Boost!
            </div>
          )}
        </div>
      )}

      {/* Falling items */}
      {playing && g.items.map(item => (
        <div
          key={item.id}
          className="absolute"
          style={{
            left: item.x - ITEM_SIZE / 2,
            top: item.y - ITEM_SIZE / 2,
            opacity: item.opacity ?? 1,
            transition: 'opacity 0.3s',
          }}
        >
          <FruitSVG type={item.type} size={ITEM_SIZE} />
        </div>
      ))}

      {/* Robo-bot */}
      {playing && g.roboBot?.active && (
        <div
          className="absolute z-30"
          style={{ left: g.roboBot.x - 30, top: 20 }}
        >
          <RoboBotSVG direction={g.roboBot.direction} />
        </div>
      )}

      {/* Catch effect */}
      {showCatchEffect && (
        <div
          className="absolute z-50 text-2xl font-bold pointer-events-none animate-countdown-pop"
          style={{
            left: showCatchEffect.x - 15,
            top: showCatchEffect.y - 20,
            color: showCatchEffect.type === 'slime' ? '#ff4444' : '#ffe066',
          }}
        >
          {showCatchEffect.type === 'slime' ? '❄️' : '+1'}
        </div>
      )}

      {/* Astro-Cat */}
      {playing && (
        <AstroCat
          x={g.catX}
          frozen={g.frozen}
          happy={!!showCatchEffect && showCatchEffect.type !== 'slime'}
        />
      )}

      {/* Frozen overlay */}
      {g.frozen && playing && (
        <div className="absolute inset-0 pointer-events-none z-20" style={{ background: 'rgba(100, 200, 255, 0.1)' }} />
      )}

      {/* Pause overlay */}
      {paused && (
        <div className="absolute inset-0 z-50 flex flex-col items-center justify-center" style={{ background: 'rgba(0,0,0,0.7)' }}>
          <h2 className="text-4xl font-bold mb-8" style={{ color: '#ffe066' }}>PAUSED</h2>
          <div className="flex flex-col gap-3">
            <button
              onClick={() => setPaused(false)}
              className="px-8 py-3 text-xl font-bold rounded-xl transition-all hover:scale-105"
              style={{ background: 'linear-gradient(135deg, #27ae60, #2ecc71)', color: 'white', border: '2px solid rgba(255,255,255,0.2)' }}
            >
              ▶ Resume
            </button>
            <button
              onClick={onMenu}
              className="px-8 py-3 text-lg font-bold rounded-xl transition-all hover:scale-105"
              style={{ background: 'rgba(255,255,255,0.1)', color: 'white', border: '1px solid rgba(255,255,255,0.2)' }}
            >
              🏠 Menu
            </button>
          </div>
        </div>
      )}

      {/* Mobile touch hint */}
      {playing && (
        <div className="absolute bottom-2 left-0 right-0 z-30 flex justify-between px-6 pointer-events-none md:hidden">
          <span style={{ color: 'rgba(255,255,255,0.3)', fontSize: 12 }}>◄ TAP LEFT</span>
          <span style={{ color: 'rgba(255,255,255,0.3)', fontSize: 12 }}>TAP RIGHT ►</span>
        </div>
      )}
    </div>
  );
};

export default GameScreen;
