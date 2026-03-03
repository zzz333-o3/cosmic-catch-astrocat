import React, { useState, useCallback } from 'react';
import MainMenu from '@/components/game/MainMenu';
import LevelSelect from '@/components/game/LevelSelect';
import GameScreen from '@/components/game/GameScreen';
import WinScreen from '@/components/game/WinScreen';
import { LEVELS } from '@/game/constants';
import { loadProgress, unlockNextLevel } from '@/game/storage';
import { PlayerProgress } from '@/game/types';

type Screen = 'menu' | 'levels' | 'playing' | 'win';

const Index = () => {
  const [screen, setScreen] = useState<Screen>('menu');
  const [currentLevel, setCurrentLevel] = useState(1);
  const [soundOn, setSoundOn] = useState(true);
  const [progress, setProgress] = useState<PlayerProgress>(loadProgress);
  const [winScore, setWinScore] = useState(0);
  const [winStars, setWinStars] = useState(0);

  const handlePlay = useCallback(() => {
    setCurrentLevel(progress.unlockedLevel);
    setScreen('playing');
  }, [progress]);

  const handleSelectLevel = useCallback((level: number) => {
    setCurrentLevel(level);
    setScreen('playing');
  }, []);

  const handleWin = useCallback((score: number) => {
    const stars = score >= 50 ? 3 : score >= 30 ? 2 : 1;
    setWinScore(score);
    setWinStars(stars);
    const updated = unlockNextLevel(currentLevel, score);
    setProgress(updated);
    setScreen('win');
  }, [currentLevel]);

  const handleNextLevel = useCallback(() => {
    const next = Math.min(currentLevel + 1, 10);
    setCurrentLevel(next);
    setScreen('playing');
  }, [currentLevel]);

  const level = LEVELS[currentLevel - 1];

  switch (screen) {
    case 'menu':
      return (
        <MainMenu
          onPlay={handlePlay}
          onLevels={() => setScreen('levels')}
          soundOn={soundOn}
          onToggleSound={() => setSoundOn(s => !s)}
        />
      );
    case 'levels':
      return (
        <LevelSelect
          progress={progress}
          onSelectLevel={handleSelectLevel}
          onBack={() => setScreen('menu')}
        />
      );
    case 'playing':
      return (
        <GameScreen
          key={`level-${currentLevel}-${Date.now()}`}
          levelId={currentLevel}
          onWin={handleWin}
          onPause={() => {}}
          onMenu={() => setScreen('menu')}
        />
      );
    case 'win':
      return (
        <WinScreen
          levelName={level.name}
          score={winScore}
          stars={winStars}
          onNextLevel={handleNextLevel}
          onMenu={() => setScreen('menu')}
          isLastLevel={currentLevel >= 10}
        />
      );
    default:
      return null;
  }
};

export default Index;
