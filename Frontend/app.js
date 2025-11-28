const API_BASE = '/api/gameItems';

let allItems = [];
let categories = [];
let rarities = [];

// Initialize the application
document.addEventListener('DOMContentLoaded', async () => {
    setupTabs();
    await loadCategories();
    await loadRarities();
    await loadItems();
});

// Tab navigation
function setupTabs() {
    const tabs = document.querySelectorAll('.tab');
    tabs.forEach(tab => {
        tab.addEventListener('click', () => {
            const targetTab = tab.dataset.tab;
            
            // Remove active class from all tabs and contents
            document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
            document.querySelectorAll('.tab-content').forEach(c => c.classList.remove('active'));
            
            // Add active class to clicked tab and corresponding content
            tab.classList.add('active');
            document.getElementById(`${targetTab}-tab`).classList.add('active');
            
            // Load data for specific tabs
            if (targetTab === 'analytics') {
                loadAnalytics();
            }
        });
    });
}

// Load categories
async function loadCategories() {
    try {
        const response = await fetch(`${API_BASE}/categories`);
        categories = await response.json();
        
        const categorySelect = document.getElementById('category');
        const categoryFilter = document.getElementById('categoryFilter');
        
        categories.forEach(cat => {
            categorySelect.innerHTML += `<option value="${cat}">${cat}</option>`;
            categoryFilter.innerHTML += `<option value="${cat}">${cat}</option>`;
        });
    } catch (error) {
        console.error('Error loading categories:', error);
    }
}

// Load rarities
async function loadRarities() {
    try {
        const response = await fetch(`${API_BASE}/rarities`);
        rarities = await response.json();
        
        const raritySelect = document.getElementById('rarity');
        const rarityFilter = document.getElementById('rarityFilter');
        
        rarities.forEach(rar => {
            raritySelect.innerHTML += `<option value="${rar}">${rar}</option>`;
            rarityFilter.innerHTML += `<option value="${rar}">${rar}</option>`;
        });
    } catch (error) {
        console.error('Error loading rarities:', error);
    }
}

// Load all items
async function loadItems() {
    try {
        const response = await fetch(API_BASE);
        allItems = await response.json();
        displayItems(allItems);
    } catch (error) {
        console.error('Error loading items:', error);
        document.getElementById('items-list').innerHTML = `
            <div class="error-message">Failed to load items. Please try again.</div>
        `;
    }
}

// Display items
function displayItems(items) {
    const container = document.getElementById('items-list');
    
    if (items.length === 0) {
        container.innerHTML = `
            <div class="loading">No items found. Create some or generate random items!</div>
        `;
        return;
    }
    
    container.innerHTML = items.map(item => `
        <div class="item-card ${item.rarity}">
            <div class="item-header">
                <div class="item-name">${item.name}</div>
                <span class="item-rarity rarity-${item.rarity}">${item.rarity}</span>
            </div>
            <div class="item-details">
                <div class="item-detail">
                    <span class="detail-label">Category:</span>
                    <span class="detail-value">${item.category}</span>
                </div>
                <div class="item-detail">
                    <span class="detail-label">Level Required:</span>
                    <span class="detail-value">${item.levelRequirement}</span>
                </div>
                <div class="item-detail">
                    <span class="detail-label">Price:</span>
                    <span class="detail-value">${item.price.toFixed(2)} 🪙</span>
                </div>
                <div class="item-detail">
                    <span class="detail-label">Created:</span>
                    <span class="detail-value">${new Date(item.createdAt).toLocaleDateString()}</span>
                </div>
            </div>
            <div class="item-actions">
                <button class="btn btn-primary" onclick="editItem(${item.id})">✏️ Edit</button>
                <button class="btn btn-danger" onclick="deleteItem(${item.id})">🗑️ Delete</button>
            </div>
        </div>
    `).join('');
}

// Filter items
function filterItems() {
    const searchTerm = document.getElementById('search').value.toLowerCase();
    const categoryFilter = document.getElementById('categoryFilter').value;
    const rarityFilter = document.getElementById('rarityFilter').value;
    
    let filtered = allItems.filter(item => {
        const matchesSearch = item.name.toLowerCase().includes(searchTerm);
        const matchesCategory = !categoryFilter || item.category === categoryFilter;
        const matchesRarity = !rarityFilter || item.rarity === rarityFilter;
        
        return matchesSearch && matchesCategory && matchesRarity;
    });
    
    displayItems(filtered);
}

// Create new item
async function createItem(event) {
    event.preventDefault();
    
    const formData = {
        name: document.getElementById('name').value,
        category: document.getElementById('category').value,
        rarity: document.getElementById('rarity').value,
        levelRequirement: parseInt(document.getElementById('level').value),
        price: parseFloat(document.getElementById('price').value)
    };
    
    try {
        const response = await fetch(API_BASE, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formData)
        });
        
        if (response.ok) {
            showMessage('✅ Item created successfully!', 'success');
            document.getElementById('create-form').reset();
            await loadItems();
            
            // Switch to items tab
            document.querySelector('[data-tab="items"]').click();
        } else {
            showMessage('❌ Failed to create item', 'error');
        }
    } catch (error) {
        console.error('Error creating item:', error);
        showMessage('❌ Error creating item', 'error');
    }
}

// Edit item
async function editItem(id) {
    const item = allItems.find(i => i.id === id);
    if (!item) return;
    
    const modalBody = document.getElementById('modal-body');
    modalBody.innerHTML = `
        <h3>Edit Item</h3>
        <form id="edit-form" onsubmit="updateItem(event, ${id})">
            <div class="form-group">
                <label for="edit-name">Item Name</label>
                <input type="text" id="edit-name" value="${item.name}" required>
            </div>
            <div class="form-group">
                <label for="edit-category">Category</label>
                <select id="edit-category" required>
                    ${categories.map(cat => `<option value="${cat}" ${cat === item.category ? 'selected' : ''}>${cat}</option>`).join('')}
                </select>
            </div>
            <div class="form-group">
                <label for="edit-rarity">Rarity</label>
                <select id="edit-rarity" required>
                    ${rarities.map(rar => `<option value="${rar}" ${rar === item.rarity ? 'selected' : ''}>${rar}</option>`).join('')}
                </select>
            </div>
            <div class="form-group">
                <label for="edit-level">Level Requirement</label>
                <input type="number" id="edit-level" value="${item.levelRequirement}" required min="1" max="100">
            </div>
            <div class="form-group">
                <label for="edit-price">Price</label>
                <input type="number" id="edit-price" value="${item.price}" required min="0" step="0.01">
            </div>
            <button type="submit" class="btn btn-primary">💾 Save Changes</button>
        </form>
    `;
    
    document.getElementById('modal').style.display = 'block';
}

// Update item
async function updateItem(event, id) {
    event.preventDefault();
    
    const formData = {
        name: document.getElementById('edit-name').value,
        category: document.getElementById('edit-category').value,
        rarity: document.getElementById('edit-rarity').value,
        levelRequirement: parseInt(document.getElementById('edit-level').value),
        price: parseFloat(document.getElementById('edit-price').value)
    };
    
    try {
        const response = await fetch(`${API_BASE}/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formData)
        });
        
        if (response.ok) {
            showMessage('✅ Item updated successfully!', 'success');
            closeModal();
            await loadItems();
        } else {
            showMessage('❌ Failed to update item', 'error');
        }
    } catch (error) {
        console.error('Error updating item:', error);
        showMessage('❌ Error updating item', 'error');
    }
}

// Delete item
async function deleteItem(id) {
    if (!confirm('Are you sure you want to delete this item?')) return;
    
    try {
        const response = await fetch(`${API_BASE}/${id}`, {
            method: 'DELETE'
        });
        
        if (response.ok) {
            showMessage('✅ Item deleted successfully!', 'success');
            await loadItems();
        } else {
            showMessage('❌ Failed to delete item', 'error');
        }
    } catch (error) {
        console.error('Error deleting item:', error);
        showMessage('❌ Error deleting item', 'error');
    }
}

// Generate random items
async function generateItems() {
    const count = prompt('How many items would you like to generate? (1-100)', '10');
    if (!count) return;
    
    const numCount = parseInt(count);
    if (isNaN(numCount) || numCount < 1 || numCount > 100) {
        alert('Please enter a number between 1 and 100');
        return;
    }
    
    try {
        const response = await fetch(`${API_BASE}/generate`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ count: numCount })
        });
        
        if (response.ok) {
            showMessage(`✅ Generated ${numCount} items successfully!`, 'success');
            await loadItems();
        } else {
            showMessage('❌ Failed to generate items', 'error');
        }
    } catch (error) {
        console.error('Error generating items:', error);
        showMessage('❌ Error generating items', 'error');
    }
}

// Load analytics
async function loadAnalytics() {
    try {
        const response = await fetch(`${API_BASE}/analytics`);
        const analytics = await response.json();
        
        const container = document.getElementById('analytics-content');
        container.innerHTML = `
            <div class="analytics-grid">
                <div class="analytics-card">
                    <h3>📊 Overview</h3>
                    <div class="stat-item">
                        <span class="stat-label">Total Items:</span>
                        <span class="stat-value">${analytics.totalItems}</span>
                    </div>
                    <div class="stat-item">
                        <span class="stat-label">Average Price:</span>
                        <span class="stat-value">${analytics.averagePrice.toFixed(2)} 🪙</span>
                    </div>
                </div>
                
                <div class="analytics-card">
                    <h3>📦 Items by Category</h3>
                    ${Object.entries(analytics.itemsByCategory || {})
                        .sort((a, b) => b[1] - a[1])
                        .map(([cat, count]) => `
                            <div class="stat-item">
                                <span class="stat-label">${cat}:</span>
                                <span class="stat-value">${count}</span>
                            </div>
                        `).join('')}
                </div>
                
                <div class="analytics-card">
                    <h3>✨ Items by Rarity</h3>
                    ${Object.entries(analytics.itemsByRarity || {})
                        .sort((a, b) => rarities.indexOf(a[0]) - rarities.indexOf(b[0]))
                        .map(([rar, count]) => `
                            <div class="stat-item">
                                <span class="stat-label ${rar}">${rar}:</span>
                                <span class="stat-value">${count}</span>
                            </div>
                        `).join('')}
                </div>
                
                ${analytics.highestLevelItem ? `
                    <div class="analytics-card">
                        <h3>🏆 Highest Level Item</h3>
                        <div class="stat-item">
                            <span class="stat-label">Name:</span>
                            <span class="stat-value">${analytics.highestLevelItem.name}</span>
                        </div>
                        <div class="stat-item">
                            <span class="stat-label">Level:</span>
                            <span class="stat-value">${analytics.highestLevelItem.levelRequirement}</span>
                        </div>
                        <div class="stat-item">
                            <span class="stat-label">Rarity:</span>
                            <span class="stat-value">${analytics.highestLevelItem.rarity}</span>
                        </div>
                    </div>
                ` : ''}
            </div>
        `;
    } catch (error) {
        console.error('Error loading analytics:', error);
        document.getElementById('analytics-content').innerHTML = `
            <div class="error-message">Failed to load analytics. Please try again.</div>
        `;
    }
}

// Modal functions
function closeModal() {
    document.getElementById('modal').style.display = 'none';
}

window.onclick = function(event) {
    const modal = document.getElementById('modal');
    if (event.target === modal) {
        modal.style.display = 'none';
    }
}

// Show message
function showMessage(message, type) {
    const existingMessage = document.querySelector('.success-message, .error-message');
    if (existingMessage) {
        existingMessage.remove();
    }
    
    const messageDiv = document.createElement('div');
    messageDiv.className = type === 'success' ? 'success-message' : 'error-message';
    messageDiv.textContent = message;
    
    const container = document.querySelector('.container');
    container.insertBefore(messageDiv, container.firstChild);
    
    setTimeout(() => {
        messageDiv.remove();
    }, 3000);
}
