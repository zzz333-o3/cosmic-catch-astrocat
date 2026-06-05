import React from 'react';
import StarBackground from './StarBackground';
import AstroCat from './AstroCat';
import { Volume2, VolumeX } from 'lucide-react';

interface MainMenuProps {
  onPlay: () => void;
  onLevels: () => void;
  soundOn: boolean;
  onToggleSound: () => void;
}

const MainMenu: React.FC<MainMenuProps> = ({ onPlay, onLevels, soundOn, onToggleSound }) => {
  return (
    <div
      className="game-container flex flex-col items-center justify-center"
      style={{ background: 'linear-gradient(180deg, #0c0032 0%, #190061 40%, #240090 70%, #3500d3 100%)' }}
    >
      <StarBackground />

      {/* Title */}
      <div className="relative z-10 flex flex-col items-center gap-6">
        <h1
          className="text-5xl md:text-7xl font-extrabold tracking-wider"
          style={{
            background: 'linear-gradient(135deg, #ffe066, #ff6b6b, #a259ff)',
            WebkitBackgroundClip: 'text',
            WebkitTextFillColor: 'transparent',
            textShadow: 'none',
            filter: 'drop-shadow(0 0 20px rgba(162, 89, 255, 0.5))',
          }}
        >
          COSMIC FRUITS
        </h1>

        <p className="text-lg md:text-xl font-medium" style={{ color: '#c8b6ff' }}>
          Catch the stars, dodge the slime!
        </p>

        {/* Astro-Cat */}
        <div className="relative animate-float" style={{ width: 120, height: 140 }}>
          <AstroCat x={60} waving happy size={120} />
        </div>

        {/* Buttons */}
        <div className="flex flex-col gap-4 mt-4">
          <button
            onClick={onPlay}
            className="px-12 py-4 text-2xl font-bold rounded-2xl transition-all duration-200 hover:scale-110 active:scale-95"
            style={{
              background: 'linear-gradient(135deg, #ff6b6b, #ee5a24)',
              color: 'white',
              boxShadow: '0 0 30px rgba(238, 90, 36, 0.5), inset 0 2px 0 rgba(255,255,255,0.2)',
              border: '2px solid rgba(255,255,255,0.2)',
            }}
          >
            🚀 PLAY
          </button>

          <button
            onClick={onLevels}
            className="px-12 py-3 text-xl font-bold rounded-2xl transition-all duration-200 hover:scale-105 active:scale-95"
            style={{
              background: 'linear-gradient(135deg, #a259ff, #7c3aed)',
              color: 'white',
              boxShadow: '0 0 20px rgba(124, 58, 237, 0.4), inset 0 2px 0 rgba(255,255,255,0.15)',
              border: '2px solid rgba(255,255,255,0.15)',
            }}
          >
            🪐 LEVELS
          </button>
        </div>

        {/* Sound toggle */}
        <button
          onClick={onToggleSound}
          className="mt-4 p-3 rounded-full transition-all hover:scale-110"
          style={{
            background: 'rgba(255,255,255,0.1)',
            border: '1px solid rgba(255,255,255,0.2)',
            color: 'white',
          }}
        >
          {soundOn ? <Volume2 size={24} /> : <VolumeX size={24} />}
        </button>
      </div>
    </div>
  );
};

export default MainMenu;
