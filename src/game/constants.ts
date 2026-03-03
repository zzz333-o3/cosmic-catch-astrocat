import { LevelConfig } from './types';

export const GAME_WIDTH = 800;
export const GAME_HEIGHT = 600;
export const CAT_WIDTH = 60;
export const CAT_HEIGHT = 70;
export const CAT_SPEED = 6;
export const ITEM_SIZE = 36;
export const FREEZE_DURATION = 60; // frames (1 second at 60fps)
export const MAGNET_DURATION = 300; // 5 seconds
export const STAR_BOOST_DURATION = 180; // 3 seconds
export const ROBO_BOT_INTERVAL_MIN = 1800; // 30 seconds
export const ROBO_BOT_INTERVAL_MAX = 2400; // 40 seconds

export const LEVELS: LevelConfig[] = [
  {
    id: 1, name: 'Green Terra',
    background: 'linear-gradient(180deg, #1a472a 0%, #2d5a27 40%, #4a7c3f 100%)',
    fruitSpeed: 2, spawnRate: 80, fruitsNeeded: 15,
    hasSlime: false, slimeChance: 0, hasRoboBot: false,
    hasSway: false, hasFade: false, itemDensity: 1,
  },
  {
    id: 2, name: 'Orange Dune',
    background: 'linear-gradient(180deg, #c2703a 0%, #e8a84c 40%, #f5d38e 100%)',
    fruitSpeed: 2.5, spawnRate: 70, fruitsNeeded: 20,
    hasSlime: false, slimeChance: 0, hasRoboBot: false,
    hasSway: false, hasFade: false, itemDensity: 1.2,
  },
  {
    id: 3, name: 'Mechano',
    background: 'linear-gradient(180deg, #2c2c2c 0%, #4a4a5a 40%, #6b6b7a 100%)',
    fruitSpeed: 3, spawnRate: 65, fruitsNeeded: 25,
    hasSlime: false, slimeChance: 0, hasRoboBot: true,
    hasSway: false, hasFade: false, itemDensity: 1.3,
  },
  {
    id: 4, name: 'Neon City',
    background: 'linear-gradient(180deg, #0a0a1a 0%, #1a0a2e 40%, #2a1a3e 100%)',
    fruitSpeed: 3.5, spawnRate: 55, fruitsNeeded: 30,
    hasSlime: false, slimeChance: 0, hasRoboBot: true,
    hasSway: false, hasFade: false, itemDensity: 1.5,
  },
  {
    id: 5, name: 'Ice Igloo',
    background: 'linear-gradient(180deg, #a8d5e2 0%, #c5e8f0 40%, #e8f4f8 100%)',
    fruitSpeed: 3, spawnRate: 60, fruitsNeeded: 30,
    hasSlime: true, slimeChance: 0.15, hasRoboBot: true,
    hasSway: false, hasFade: false, itemDensity: 1.4,
  },
  {
    id: 6, name: 'Crystallia',
    background: 'linear-gradient(180deg, #2a0845 0%, #6441a5 40%, #9b59b6 100%)',
    fruitSpeed: 3.5, spawnRate: 45, fruitsNeeded: 35,
    hasSlime: true, slimeChance: 0.18, hasRoboBot: true,
    hasSway: false, hasFade: false, itemDensity: 1.8,
  },
  {
    id: 7, name: 'Aquamarine',
    background: 'linear-gradient(180deg, #006994 0%, #00a8cc 40%, #45c4e8 100%)',
    fruitSpeed: 2.8, spawnRate: 50, fruitsNeeded: 35,
    hasSlime: true, slimeChance: 0.2, hasRoboBot: true,
    hasSway: true, hasFade: false, itemDensity: 1.6,
  },
  {
    id: 8, name: 'Magma Prime',
    background: 'linear-gradient(180deg, #1a0000 0%, #8b0000 40%, #ff4500 100%)',
    fruitSpeed: 4, spawnRate: 45, fruitsNeeded: 40,
    hasSlime: true, slimeChance: 0.3, hasRoboBot: true,
    hasSway: false, hasFade: false, itemDensity: 1.7,
  },
  {
    id: 9, name: 'Foggy Void',
    background: 'linear-gradient(180deg, #0a0a0a 0%, #1a1a2e 40%, #16213e 100%)',
    fruitSpeed: 4, spawnRate: 45, fruitsNeeded: 40,
    hasSlime: true, slimeChance: 0.25, hasRoboBot: true,
    hasSway: false, hasFade: true, itemDensity: 1.8,
  },
  {
    id: 10, name: 'Heart of Galaxy',
    background: 'linear-gradient(180deg, #0c0032 0%, #190061 30%, #240090 60%, #3500d3 100%)',
    fruitSpeed: 5, spawnRate: 35, fruitsNeeded: 50,
    hasSlime: true, slimeChance: 0.3, hasRoboBot: true,
    hasSway: false, hasFade: false, itemDensity: 2,
  },
];

export const FRUIT_TYPES = ['apple', 'orange', 'banana', 'grape', 'star-fruit'] as const;
