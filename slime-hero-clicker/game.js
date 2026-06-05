// Game Configuration & State
const CONFIG = {
    BASE_HP: 10,
    GROWTH_MUL: 1.2,
    BOSS_MUL: 5,
    BOSS_TIME: 30,
    SAVE_KEY: 'slime_hero_save'
};

let gameState = {
    gold: 0,
    wave: 1,
    mutationPhase: 1,
    clickLevel: 1,
    upgrades: {
        peasant: 0,
        archer: 0,
        knight: 0,
        mage: 0,
        ballista: 0,
        dragon: 0
    },
    lastSaveTime: Date.now()
};

const UPGRADES = [
    { id: 'peasant', name: 'Pitchfork Peasant', baseCost: 15, dps: 1, icon: '🧑‍🌾' },
    { id: 'archer', name: 'Militia Archer', baseCost: 100, dps: 5, icon: '🏹' },
    { id: 'knight', name: 'Royal Knight', baseCost: 500, dps: 25, icon: '⚔️' },
    { id: 'mage', name: 'Battle Mage', baseCost: 3000, dps: 120, icon: '🧙‍♂️' },
    { id: 'ballista', name: 'Siege Ballista', baseCost: 15000, dps: 500, icon: '🏗️' },
    { id: 'dragon', name: 'Pet Dragon', baseCost: 100000, dps: 3000, icon: '🐉' }
];

const MUTATIONS = [
    { name: 'Basic Slime', filter: 'none', img: 'C:/Users/zzzzz/.gemini/antigravity/brain/7669ab37-d0e9-4f39-bbf8-8bbd29c3701f/basic_slime_green_1775726110475.png' },
    { name: 'Toxic Slime', filter: 'none', img: 'C:/Users/zzzzz/.gemini/antigravity/brain/7669ab37-d0e9-4f39-bbf8-8bbd29c3701f/toxic_slime_purple_1775728071836.png' },
    { name: 'Armored Slime', filter: 'grayscale(1) contrast(1.2)', img: null },
    { name: 'Magma Slime', filter: 'none', img: 'C:/Users/zzzzz/.gemini/antigravity/brain/7669ab37-d0e9-4f39-bbf8-8bbd29c3701f/magma_slime_red_1775728111317.png' },
    { name: 'Frost Slime', filter: 'hue-rotate(180deg) brightness(1.2)', img: null },
    { name: 'Crystal Slime', filter: 'hue-rotate(200deg) saturate(3)', img: null },
    { name: 'Shadow Slime', filter: 'brightness(0.2) drop-shadow(0 0 10px red)', img: null },
    { name: 'Cyber Slime', filter: 'hue-rotate(150deg) invert(0.2)', img: null },
    { name: 'Golden Slime', filter: 'sepia(1) saturate(10) brightness(1.2)', img: null },
    { name: 'Cosmic Slime', filter: 'none', img: 'C:/Users/zzzzz/.gemini/antigravity/brain/7669ab37-d0e9-4f39-bbf8-8bbd29c3701f/cosmic_slime_galaxy_1775726135276.png' }
];

let currentHP = 0;
let maxHP = 0;
let bossTimer = 0;
let bossInterval = null;

// DOM Elements
const elements = {
    gold: document.getElementById('gold-count'),
    wave: document.getElementById('wave-count'),
    phase: document.getElementById('mutation-phase'),
    hpFill: document.getElementById('hp-bar-fill'),
    hpText: document.getElementById('hp-text'),
    monster: document.getElementById('monster-sprite'),
    upgradeList: document.getElementById('upgrade-list'),
    dps: document.getElementById('current-dps'),
    timer: document.getElementById('boss-timer')
};

// --- Math & Formatting ---

function formatNumber(num) {
    if (num >= 1e9) return (num / 1e9).toFixed(1) + 'B';
    if (num >= 1e6) return (num / 1e6).toFixed(1) + 'M';
    if (num >= 1e3) return (num / 1e3).toFixed(1) + 'K';
    return Math.floor(num).toString();
}

function calculateMaxHP(wave) {
    let hp = CONFIG.BASE_HP * Math.pow(CONFIG.GROWTH_MUL, wave - 1);
    if (wave % 10 === 0) hp *= CONFIG.BOSS_MUL;
    return Math.floor(hp);
}

function getClickDamage() {
    return gameState.clickLevel;
}

function getTotalDPS() {
    let dps = 0;
    UPGRADES.forEach(u => {
        dps += gameState.upgrades[u.id] * u.dps;
    });
    return dps;
}

function getUpgradeCost(u) {
    const level = gameState.upgrades[u.id] || 0;
    return Math.floor(u.baseCost * Math.pow(1.15, level));
}

// --- Game Logic ---

function loadMonster() {
    maxHP = calculateMaxHP(gameState.wave);
    currentHP = maxHP;
    updateUI();
    
    // Mutation Visuals
    const phaseIndex = Math.min(Math.floor((gameState.wave - 1) / 10), MUTATIONS.length - 1);
    const mutation = MUTATIONS[phaseIndex];
    
    if (mutation.img) {
        elements.monster.src = mutation.img;
        elements.monster.style.filter = 'none';
    } else {
        elements.monster.src = MUTATIONS[0].img; // Fallback to basic
        elements.monster.style.filter = mutation.filter;
    }

    // Boss handling
    if (gameState.wave % 10 === 0) {
        startBoss();
    } else {
        stopBoss();
    }
}

function dealDamage(amount, isClick = false) {
    currentHP -= amount;
    
    if (isClick) {
        spawnFloatingText(amount);
    }

    if (currentHP <= 0) {
        rewardGold();
        nextWave();
    }
    
    updateUI();
}

function rewardGold() {
    let gold = maxHP * 0.2; // 20% of HP as gold
    if (gameState.wave % 10 === 0) gold *= 2;
    gameState.gold += Math.floor(gold);
}

function nextWave() {
    gameState.wave++;
    gameState.mutationPhase = Math.floor((gameState.wave - 1) / 10) + 1;
    loadMonster();
    saveGame();
}

function startBoss() {
    elements.timer.classList.remove('hidden');
    bossTimer = CONFIG.BOSS_TIME;
    elements.timer.innerText = bossTimer + 's';
    
    if (bossInterval) clearInterval(bossInterval);
    bossInterval = setInterval(() => {
        bossTimer--;
        elements.timer.innerText = bossTimer + 's';
        if (bossTimer <= 0) {
            failBoss();
        }
    }, 1000);
}

function stopBoss() {
    elements.timer.classList.add('hidden');
    if (bossInterval) clearInterval(bossInterval);
}

function failBoss() {
    stopBoss();
    gameState.wave = Math.max(1, gameState.wave - 1);
    loadMonster();
    alert("TIME'S UP! The monster escaped. Retreating...");
}

// --- UI & Effects ---

function updateUI() {
    elements.gold.innerText = formatNumber(gameState.gold);
    elements.wave.innerText = (gameState.wave % 10 || 10);
    elements.phase.innerText = gameState.mutationPhase;
    elements.dps.innerText = `DPS: ${formatNumber(getTotalDPS())}`;
    
    const hpPercent = (currentHP / maxHP) * 100;
    elements.hpFill.style.width = Math.max(0, hpPercent) + '%';
    elements.hpText.innerText = `${formatNumber(currentHP)} / ${formatNumber(maxHP)}`;

    renderShop();
}

function renderShop() {
    // Inject Active Upgrade card first (Player Weapon)
    const activeCost = Math.floor(10 * Math.pow(1.5, gameState.clickLevel - 1));
    let shopHTML = `
        <div class="upgrade-card ${gameState.gold < activeCost ? 'disabled' : ''}" onclick="buyClick()">
            <div class="upgrade-icon">⚔️</div>
            <div class="upgrade-info">
                <div class="upgrade-name">Player Strength (Lv.${gameState.clickLevel})</div>
                <div class="upgrade-stats">+1 DMG per click</div>
            </div>
            <button class="buy-btn">
                <span>BUY</span>
                ${formatNumber(activeCost)}
            </button>
        </div>
    `;

    // Passive Upgrades
    UPGRADES.forEach(u => {
        const cost = getUpgradeCost(u);
        const level = gameState.upgrades[u.id];
        shopHTML += `
            <div class="upgrade-card ${gameState.gold < cost ? 'disabled' : ''}" onclick="buyUpgrade('${u.id}')">
                <div class="upgrade-icon">${u.icon}</div>
                <div class="upgrade-info">
                    <div class="upgrade-name">${u.name} (Lv.${level})</div>
                    <div class="upgrade-stats">+${u.dps} DPS</div>
                </div>
                <button class="buy-btn">
                    <span>BUY</span>
                    ${formatNumber(cost)}
                </button>
            </div>
        `;
    });

    elements.upgradeList.innerHTML = shopHTML;
}

function spawnFloatingText(amount) {
    const text = document.createElement('div');
    text.className = 'floating-text';
    text.innerText = `-${formatNumber(amount)}`;
    
    const x = 50 + (Math.random() * 20 - 10);
    const y = 50 + (Math.random() * 20 - 10);
    text.style.left = x + '%';
    text.style.top = y + '%';
    
    document.getElementById('click-layer').appendChild(text);
    setTimeout(() => text.remove(), 800);
}

// --- Interaction ---

window.buyClick = function() {
    const cost = Math.floor(10 * Math.pow(1.5, gameState.clickLevel - 1));
    if (gameState.gold >= cost) {
        gameState.gold -= cost;
        gameState.clickLevel++;
        updateUI();
    }
};

window.buyUpgrade = function(id) {
    const u = UPGRADES.find(u => u.id === id);
    const cost = getUpgradeCost(u);
    if (gameState.gold >= cost) {
        gameState.gold -= cost;
        gameState.upgrades[id]++;
        updateUI();
    }
};

elements.monster.addEventListener('click', () => {
    dealDamage(getClickDamage(), true);
});

// --- Lifecycle ---

function saveGame() {
    gameState.lastSaveTime = Date.now();
    localStorage.setItem(CONFIG.SAVE_KEY, JSON.stringify(gameState));
}

function loadGame() {
    const saved = localStorage.getItem(CONFIG.SAVE_KEY);
    if (saved) {
        gameState = { ...gameState, ...JSON.parse(saved) };
        handleOfflineProgress();
    }
    loadMonster();
}

function handleOfflineProgress() {
    const now = Date.now();
    const diffSec = Math.floor((now - gameState.lastSaveTime) / 1000);
    const capSec = 24 * 3600;
    const actualSec = Math.min(diffSec, capSec);
    
    if (actualSec > 60) {
        const dps = getTotalDPS();
        const goldEarned = dps * actualSec * 0.5; // Offline profit reduced slightly
        if (goldEarned > 0) {
            gameState.gold += goldEarned;
            showOfflinePopup(goldEarned);
        }
    }
}

function showOfflinePopup(amount) {
    const popup = document.getElementById('offline-popup');
    document.getElementById('offline-gold').innerText = formatNumber(amount);
    popup.classList.remove('hidden');
    document.getElementById('claim-btn').onclick = () => popup.classList.add('hidden');
}

// Tick Loop (Passive Damage)
setInterval(() => {
    const dps = getTotalDPS();
    if (dps > 0) {
        dealDamage(dps / 10); // Tick every 100ms
    }
}, 100);

// Auto Save
setInterval(saveGame, 5000);

loadGame();
