import React from 'react';
import StarBackground from './StarBackground';
import { LEVELS } from '@/game/constants';
import { PlayerProgress } from '@/game/types';
import { Lock } from 'lucide-react';

interface LevelSelectProps {
  progress: PlayerProgress;
  onSelectLevel: (level: number) => void;
  onBack: () => void;
}

const PLANET_COLORS = [
  ['#2d5a27', '#4a7c3f'],
  ['#e8a84c', '#f5d38e'],
  ['#6b6b7a', '#95a5a6'],
  ['#a259ff', '#ff6bff'],
  ['#a8d5e2', '#e8f4f8'],
  ['#9b59b6', '#c39bd3'],
  ['#00a8cc', '#45c4e8'],
  ['#ff4500', '#ff6347'],
  ['#1a1a2e', '#4a4a6a'],
  ['#3500d3', '#7c3aed'],
];

const LevelSelect: React.FC<LevelSelectProps> = ({ progress, onSelectLevel, onBack }) => {
  return (
    <div
      className="game-container flex flex-col items-center justify-center p-4"
      style={{ background: 'linear-gradient(180deg, #0c0032 0%, #190061 50%, #0f0f2e 100%)' }}
    >
      <StarBackground />

      <div className="relative z-10 flex flex-col items-center gap-6 w-full max-w-lg">
        <h2
          className="text-3xl md:text-4xl font-extrabold"
          style={{
            background: 'linear-gradient(135deg, #ffe066, #ff6b6b)',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
          }}
        >
          🪐 Planet Map
        </h2>

        <div className="grid grid-cols-5 gap-3 md:gap-4">
          {LEVELS.map((level, i) => {
            const unlocked = level.id <= progress.unlockedLevel;
            const stars = progress.stars[level.id] || 0;
            return (
              <button
                key={level.id}
                onClick={() => unlocked && onSelectLevel(level.id)}
                disabled={!unlocked}
                className="relative flex flex-col items-center gap-1 transition-all duration-200"
                style={{ cursor: unlocked ? 'pointer' : 'not-allowed' }}
              >
                <div
                  className="w-14 h-14 md:w-16 md:h-16 rounded-full flex items-center justify-center text-lg font-bold transition-transform duration-200"
                  style={{
                    background: unlocked
                      ? `linear-gradient(135deg, ${PLANET_COLORS[i][0]}, ${PLANET_COLORS[i][1]})`
                      : '#333',
                    boxShadow: unlocked
                      ? `0 0 15px ${PLANET_COLORS[i][0]}80, inset 0 -3px 6px rgba(0,0,0,0.3)`
                      : 'none',
                    color: unlocked ? 'white' : '#666',
                    border: unlocked ? '2px solid rgba(255,255,255,0.3)' : '2px solid #444',
                    transform: unlocked ? 'scale(1)' : 'scale(0.9)',
                  }}
                  onMouseEnter={(e) => unlocked && (e.currentTarget.style.transform = 'scale(1.15)')}
                  onMouseLeave={(e) => unlocked && (e.currentTarget.style.transform = 'scale(1)')}
                >
                  {unlocked ? level.id : <Lock size={18} />}
                </div>
                <span className="text-[10px] md:text-xs font-medium" style={{ color: unlocked ? '#c8b6ff' : '#555' }}>
                  {level.name}
                </span>
                {unlocked && stars > 0 && (
                  <div className="flex gap-0.5">
                    {[1, 2, 3].map((s) => (
                      <span key={s} style={{ fontSize: 10, opacity: s <= stars ? 1 : 0.2 }}>⭐</span>
                    ))}
                  </div>
                )}
              </button>
            );
          })}
        </div>

        <button
          onClick={onBack}
          className="mt-4 px-6 py-2 text-lg font-bold rounded-xl transition-all hover:scale-105"
          style={{
            background: 'rgba(255,255,255,0.1)',
            color: 'white',
            border: '1px solid rgba(255,255,255,0.2)',
          }}
        >
          ← Back
        </button>
      </div>
    </div>
  );
};

export default LevelSelect;
