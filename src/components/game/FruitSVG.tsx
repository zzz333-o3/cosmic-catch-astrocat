import React from 'react';
import { ItemType } from '@/game/types';

interface FruitSVGProps {
  type: ItemType;
  size?: number;
  opacity?: number;
}

const FruitSVG: React.FC<FruitSVGProps> = ({ type, size = 36, opacity = 1 }) => {
  const s = size;
  return (
    <svg width={s} height={s} viewBox="0 0 36 36" opacity={opacity}>
      {type === 'apple' && (
        <g>
          <circle cx="18" cy="20" r="14" fill="#e74c3c" />
          <circle cx="12" cy="16" r="3" fill="#ff6b6b" opacity="0.6" />
          <path d="M18,6 Q20,2 22,6" stroke="#27ae60" strokeWidth="2" fill="none" />
          <ellipse cx="20" cy="6" rx="4" ry="2" fill="#27ae60" />
        </g>
      )}
      {type === 'orange' && (
        <g>
          <circle cx="18" cy="20" r="13" fill="#f39c12" />
          <circle cx="14" cy="17" r="2.5" fill="#f1c40f" opacity="0.5" />
          <circle cx="18" cy="8" r="2" fill="#27ae60" />
        </g>
      )}
      {type === 'banana' && (
        <g>
          <path d="M8,28 Q4,18 14,8 Q18,4 22,8 Q16,18 12,28 Z" fill="#f1c40f" />
          <path d="M10,26 Q6,18 14,10" stroke="#e6b800" strokeWidth="1.5" fill="none" />
        </g>
      )}
      {type === 'grape' && (
        <g>
          <circle cx="14" cy="18" r="5" fill="#8e44ad" />
          <circle cx="22" cy="18" r="5" fill="#8e44ad" />
          <circle cx="18" cy="24" r="5" fill="#9b59b6" />
          <circle cx="10" cy="24" r="5" fill="#8e44ad" />
          <circle cx="26" cy="24" r="5" fill="#8e44ad" />
          <circle cx="14" cy="30" r="5" fill="#9b59b6" />
          <circle cx="22" cy="30" r="5" fill="#9b59b6" />
          <path d="M18,8 L18,14" stroke="#27ae60" strokeWidth="2" />
          <ellipse cx="20" cy="8" rx="3" ry="1.5" fill="#27ae60" />
        </g>
      )}
      {type === 'star-fruit' && (
        <g>
          <polygon
            points="18,4 21,14 32,14 23,20 26,30 18,24 10,30 13,20 4,14 15,14"
            fill="#f1c40f"
            stroke="#e6b800"
            strokeWidth="1"
          />
          <polygon
            points="18,8 20,14 26,14 21,18 23,24 18,20 13,24 15,18 10,14 16,14"
            fill="#ffe066"
            opacity="0.5"
          />
        </g>
      )}
      {type === 'slime' && (
        <g>
          <ellipse cx="18" cy="24" rx="14" ry="10" fill="#27ae60" />
          <ellipse cx="18" cy="22" rx="12" ry="8" fill="#2ecc71" />
          <circle cx="12" cy="20" r="3" fill="#1a1a2e" />
          <circle cx="24" cy="20" r="3" fill="#1a1a2e" />
          <circle cx="11" cy="19" r="1" fill="white" />
          <circle cx="23" cy="19" r="1" fill="white" />
          <path d="M14,26 Q18,30 22,26" stroke="#1a1a2e" strokeWidth="1.5" fill="none" />
          {/* Drip effects */}
          <ellipse cx="8" cy="28" rx="3" ry="4" fill="#2ecc71" />
          <ellipse cx="28" cy="28" rx="3" ry="4" fill="#2ecc71" />
        </g>
      )}
      {type === 'magnet' && (
        <g>
          <path d="M8,10 L8,24 Q8,32 18,32 Q28,32 28,24 L28,10" stroke="#e74c3c" strokeWidth="5" fill="none" />
          <rect x="5" y="6" width="8" height="6" fill="#c0c0c0" rx="1" />
          <rect x="23" y="6" width="8" height="6" fill="#c0c0c0" rx="1" />
          <path d="M8,10 L8,24 Q8,32 18,32 Q28,32 28,24 L28,10" stroke="#3498db" strokeWidth="2" fill="none" opacity="0.5" />
        </g>
      )}
      {type === 'golden-star' && (
        <g>
          <polygon
            points="18,2 22,12 34,12 24,20 28,32 18,24 8,32 12,20 2,12 14,12"
            fill="#ffd700"
            stroke="#ffaa00"
            strokeWidth="1.5"
          />
          <polygon
            points="18,6 21,13 28,13 22,18 24,26 18,21 12,26 14,18 8,13 15,13"
            fill="#ffec80"
            opacity="0.6"
          />
          <circle cx="18" cy="16" r="3" fill="white" opacity="0.4" />
        </g>
      )}
    </svg>
  );
};

export default FruitSVG;
