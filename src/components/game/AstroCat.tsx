import React from 'react';

interface AstroCatProps {
  x: number;
  frozen?: boolean;
  happy?: boolean;
  waving?: boolean;
  size?: number;
}

const AstroCat: React.FC<AstroCatProps> = ({ x, frozen, happy, waving, size = 60 }) => {
  const s = size;
  return (
    <svg
      width={s}
      height={s * 1.17}
      viewBox="0 0 60 70"
      style={{
        position: 'absolute',
        left: x - s / 2,
        bottom: 10,
        transition: frozen ? 'none' : 'left 0.05s linear',
        filter: frozen ? 'hue-rotate(120deg) brightness(1.5)' : 'none',
      }}
    >
      {/* Body */}
      <ellipse cx="30" cy="50" rx="22" ry="18" fill="#f5a623" />
      {/* Head */}
      <circle cx="30" cy="28" r="18" fill="#f5a623" />
      {/* Left ear */}
      <polygon points="14,14 10,0 22,10" fill="#f5a623" />
      <polygon points="15,13 12,3 21,11" fill="#ffb6c1" />
      {/* Right ear */}
      <polygon points="46,14 50,0 38,10" fill="#f5a623" />
      <polygon points="45,13 48,3 39,11" fill="#ffb6c1" />
      {/* Eyes */}
      <ellipse cx="22" cy="26" rx="4" ry={happy ? 2 : 4.5} fill="#1a1a2e" />
      <ellipse cx="38" cy="26" rx="4" ry={happy ? 2 : 4.5} fill="#1a1a2e" />
      {!happy && (
        <>
          <circle cx="20" cy="24" r="1.5" fill="white" />
          <circle cx="36" cy="24" r="1.5" fill="white" />
        </>
      )}
      {/* Nose */}
      <ellipse cx="30" cy="32" rx="2.5" ry="2" fill="#ffb6c1" />
      {/* Mouth */}
      {happy ? (
        <path d="M24,35 Q30,42 36,35" stroke="#1a1a2e" strokeWidth="1.5" fill="none" />
      ) : frozen ? (
        <circle cx="30" cy="37" r="3" fill="#1a1a2e" />
      ) : (
        <path d="M26,36 Q30,39 34,36" stroke="#1a1a2e" strokeWidth="1.2" fill="none" />
      )}
      {/* Whiskers */}
      <line x1="5" y1="30" x2="18" y2="32" stroke="#1a1a2e" strokeWidth="0.8" />
      <line x1="5" y1="35" x2="18" y2="34" stroke="#1a1a2e" strokeWidth="0.8" />
      <line x1="42" y1="32" x2="55" y2="30" stroke="#1a1a2e" strokeWidth="0.8" />
      <line x1="42" y1="34" x2="55" y2="35" stroke="#1a1a2e" strokeWidth="0.8" />
      {/* Helmet visor (space theme) */}
      <path d="M12,20 Q12,8 30,8 Q48,8 48,20" stroke="#88ccff" strokeWidth="2" fill="none" opacity="0.5" />
      {/* Tail */}
      <path d="M52,50 Q60,40 55,30" stroke="#f5a623" strokeWidth="4" fill="none" strokeLinecap="round" />
      {/* Waving paw */}
      {waving && (
        <g className="animate-wave" style={{ transformOrigin: '48px 45px' }}>
          <ellipse cx="52" cy="42" rx="5" ry="4" fill="#f5a623" />
          <ellipse cx="52" cy="42" rx="3" ry="2.5" fill="#ffcb77" />
        </g>
      )}
      {/* Basket */}
      <path d="M10,58 L14,68 L46,68 L50,58" fill="#8B4513" stroke="#5a2d0c" strokeWidth="1.5" />
      <path d="M12,63 L48,63" stroke="#5a2d0c" strokeWidth="0.8" />
      {/* Frozen effect */}
      {frozen && (
        <>
          <circle cx="15" cy="20" r="2" fill="#88eeff" opacity="0.7" />
          <circle cx="45" cy="25" r="1.5" fill="#88eeff" opacity="0.6" />
          <circle cx="25" cy="55" r="2.5" fill="#88eeff" opacity="0.5" />
        </>
      )}
    </svg>
  );
};

export default AstroCat;
