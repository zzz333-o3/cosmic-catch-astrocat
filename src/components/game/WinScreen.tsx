import React from 'react';
import StarBackground from './StarBackground';

interface WinScreenProps {
  levelName: string;
  score: number;
  stars: number;
  onNextLevel: () => void;
  onMenu: () => void;
  isLastLevel: boolean;
}

const WinScreen: React.FC<WinScreenProps> = ({ levelName, score, stars, onNextLevel, onMenu, isLastLevel }) => {
  return (
    <div
      className="game-container flex flex-col items-center justify-center"
      style={{ background: 'linear-gradient(180deg, #0c0032 0%, #190061 40%, #3500d3 100%)' }}
    >
      <StarBackground />

      <div className="relative z-10 flex flex-col items-center gap-6">
        <h1
          className="text-4xl md:text-6xl font-extrabold"
          style={{
            background: 'linear-gradient(135deg, #ffe066, #ff6b6b, #a259ff)',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
            filter: 'drop-shadow(0 0 20px rgba(255, 224, 102, 0.5))',
          }}
        >
          {isLastLevel ? '🎆 YOU WIN! 🎆' : 'LEVEL COMPLETE!'}
        </h1>

        <p className="text-xl" style={{ color: '#c8b6ff' }}>{levelName}</p>

        {/* Stars */}
        <div className="flex gap-4">
          {[1, 2, 3].map((s) => (
            <span
              key={s}
              className={s <= stars ? 'animate-bounce-star' : ''}
              style={{
                fontSize: 48,
                opacity: s <= stars ? 1 : 0.2,
                animationDelay: `${s * 0.15}s`,
              }}
            >
              ⭐
            </span>
          ))}
        </div>

        <p className="text-2xl font-bold" style={{ color: '#ffe066' }}>
          Score: {score}
        </p>

        <div className="flex flex-col gap-3 mt-4">
          {!isLastLevel && (
            <button
              onClick={onNextLevel}
              className="px-10 py-3 text-xl font-bold rounded-2xl transition-all hover:scale-110 active:scale-95"
              style={{
                background: 'linear-gradient(135deg, #27ae60, #2ecc71)',
                color: 'white',
                boxShadow: '0 0 20px rgba(46, 204, 113, 0.4)',
                border: '2px solid rgba(255,255,255,0.2)',
              }}
            >
              Next Level →
            </button>
          )}

          <button
            onClick={onMenu}
            className="px-10 py-3 text-lg font-bold rounded-2xl transition-all hover:scale-105"
            style={{
              background: 'rgba(255,255,255,0.1)',
              color: 'white',
              border: '1px solid rgba(255,255,255,0.2)',
            }}
          >
            🏠 Menu
          </button>
        </div>

        {/* Fireworks for last level */}
        {isLastLevel && (
          <div className="absolute inset-0 pointer-events-none overflow-hidden">
            {Array.from({ length: 20 }, (_, i) => (
              <div
                key={i}
                className="absolute rounded-full"
                style={{
                  left: `${Math.random() * 100}%`,
                  top: `${Math.random() * 100}%`,
                  width: Math.random() * 8 + 4,
                  height: Math.random() * 8 + 4,
                  background: ['#ff6b6b', '#ffe066', '#a259ff', '#2ecc71', '#3498db'][i % 5],
                  animation: `twinkle ${Math.random() * 1 + 0.5}s ease-in-out ${Math.random() * 2}s infinite`,
                }}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  );
};

export default WinScreen;
