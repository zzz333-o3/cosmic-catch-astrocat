export type GameScreen = 'menu' | 'levels' | 'playing' | 'countdown' | 'win' | 'paused';

export type ItemType = 'apple' | 'orange' | 'banana' | 'grape' | 'star-fruit' | 'slime' | 'magnet' | 'golden-star';

export interface FallingItem {
  id: number;
  type: ItemType;
  x: number;
  y: number;
  speed: number;
  swayOffset?: number;
  swaySpeed?: number;
  opacity?: number;
}

export interface RoboBot {
  x: number;
  direction: 1 | -1;
  active: boolean;
  droppedPowerUp: boolean;
}

export interface LevelConfig {
  id: number;
  name: string;
  background: string;
  fruitSpeed: number;
  spawnRate: number;
  fruitsNeeded: number;
  hasSlime: boolean;
  slimeChance: number;
  hasRoboBot: boolean;
  hasSway: boolean;
  hasFade: boolean;
  itemDensity: number;
}

export interface GameState {
  score: number;
  progress: number;
  catX: number;
  frozen: boolean;
  frozenTimer: number;
  magnetActive: boolean;
  magnetTimer: number;
  starBoost: boolean;
  starBoostTimer: number;
  items: FallingItem[];
  roboBot: RoboBot | null;
  roboBotTimer: number;
  spawnTimer: number;
  nextItemId: number;
}

export interface PlayerProgress {
  unlockedLevel: number;
  highScores: Record<number, number>;
  stars: Record<number, number>;
}
