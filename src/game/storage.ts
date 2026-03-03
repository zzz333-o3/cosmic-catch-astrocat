import { PlayerProgress } from './types';

const STORAGE_KEY = 'cosmic-fruits-progress';

export function loadProgress(): PlayerProgress {
  try {
    const data = localStorage.getItem(STORAGE_KEY);
    if (data) return JSON.parse(data);
  } catch {}
  return { unlockedLevel: 1, highScores: {}, stars: {} };
}

export function saveProgress(progress: PlayerProgress): void {
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(progress));
  } catch {}
}

export function unlockNextLevel(currentLevel: number, score: number): PlayerProgress {
  const progress = loadProgress();
  if (currentLevel >= progress.unlockedLevel) {
    progress.unlockedLevel = Math.min(currentLevel + 1, 10);
  }
  const prev = progress.highScores[currentLevel] || 0;
  if (score > prev) progress.highScores[currentLevel] = score;
  // Star rating: 1 star = completed, 2 = 1.5x fruits, 3 = 2x fruits
  const stars = score >= 50 ? 3 : score >= 30 ? 2 : 1;
  const prevStars = progress.stars[currentLevel] || 0;
  if (stars > prevStars) progress.stars[currentLevel] = stars;
  saveProgress(progress);
  return progress;
}
