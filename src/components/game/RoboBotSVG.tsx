import React from 'react';

const RoboBotSVG: React.FC<{ direction: 1 | -1 }> = ({ direction }) => (
  <svg width="60" height="40" viewBox="0 0 60 40" style={{ transform: `scaleX(${direction})` }}>
    {/* Body */}
    <rect x="10" y="10" width="40" height="24" rx="6" fill="#7f8c8d" stroke="#5a6368" strokeWidth="1.5" />
    {/* Screen/face */}
    <rect x="16" y="14" width="28" height="12" rx="3" fill="#2c3e50" />
    {/* Eyes */}
    <circle cx="24" cy="20" r="3" fill="#00ff88" />
    <circle cx="36" cy="20" r="3" fill="#00ff88" />
    <circle cx="24" cy="19" r="1" fill="white" opacity="0.7" />
    <circle cx="36" cy="19" r="1" fill="white" opacity="0.7" />
    {/* Antenna */}
    <line x1="30" y1="10" x2="30" y2="2" stroke="#95a5a6" strokeWidth="2" />
    <circle cx="30" cy="2" r="3" fill="#e74c3c" />
    <circle cx="30" cy="2" r="1.5" fill="#ff6b6b" opacity="0.6" />
    {/* Propeller */}
    <ellipse cx="10" cy="20" rx="4" ry="8" fill="#bdc3c7" opacity="0.6" />
    <ellipse cx="50" cy="20" rx="4" ry="8" fill="#bdc3c7" opacity="0.6" />
    {/* Jet trail */}
    <ellipse cx="30" cy="36" rx="10" ry="3" fill="#3498db" opacity="0.3" />
  </svg>
);

export default RoboBotSVG;
