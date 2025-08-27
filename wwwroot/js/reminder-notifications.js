// Reminder Notification System
class ReminderNotificationSystem {
    constructor() {
        this.checkInterval = 60000; // Check every minute
        this.lastCheck = null;
        this.notificationSound = null;
        this.init();
    }
    
    init() {
        // Initialize notification sound
        this.initNotificationSound();
        
        // Start checking for reminders
        this.startReminderCheck();
        
        // Request notification permission
        this.requestNotificationPermission();
    }
    
    initNotificationSound() {
        // Create a simple notification sound
        const audioContext = new (window.AudioContext || window.webkitAudioContext)();
        const oscillator = audioContext.createOscillator();
        const gainNode = audioContext.createGain();
        
        oscillator.connect(gainNode);
        gainNode.connect(audioContext.destination);
        
        oscillator.frequency.setValueAtTime(800, audioContext.currentTime);
        oscillator.frequency.setValueAtTime(600, audioContext.currentTime + 0.1);
        oscillator.frequency.setValueAtTime(800, audioContext.currentTime + 0.2);
        
        gainNode.gain.setValueAtTime(0.1, audioContext.currentTime);
        gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.3);
        
        this.notificationSound = { oscillator, gainNode, audioContext };
    }
    
    async requestNotificationPermission() {
        if ('Notification' in window) {
            const permission = await Notification.requestPermission();
            console.log('Notification permission:', permission);
        }
    }
    
    startReminderCheck() {
        setInterval(() => {
            this.checkForUrgentReminders();
        }, this.checkInterval);
        
        // Initial check
        this.checkForUrgentReminders();
    }
    
    async checkForUrgentReminders() {
        try {
            const response = await fetch('/Reminder/GetReminders');
            const reminders = await response.json();
            
            const urgentReminders = this.filterUrgentReminders(reminders);
            
            if (urgentReminders.length > 0) {
                this.showUrgentReminderAlert(urgentReminders);
            }
            
            this.lastCheck = new Date();
        } catch (error) {
            console.error('Error checking reminders:', error);
        }
    }
    
    filterUrgentReminders(reminders) {
        const now = new Date();
        const today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        
        return reminders.filter(reminder => {
            const dueDate = new Date(reminder.dueDate);
            const isOverdue = dueDate < today;
            const isDueToday = dueDate.toDateString() === today.toDateString();
            const isCritical = reminder.priority === 'Critical';
            
            return isOverdue || isDueToday || isCritical;
        });
    }
    
    showUrgentReminderAlert(urgentReminders) {
        // Play notification sound
        this.playNotificationSound();
        
        // Show browser notification if permitted
        this.showBrowserNotification(urgentReminders);
        
        // Show in-page alert
        this.showInPageAlert(urgentReminders);
        
        // Update page title to show urgency
        this.updatePageTitle(urgentReminders.length);
    }
    
    playNotificationSound() {
        if (this.notificationSound) {
            const { oscillator, gainNode, audioContext } = this.notificationSound;
            
            oscillator.start();
            oscillator.stop(audioContext.currentTime + 0.3);
        }
    }
    
    showBrowserNotification(urgentReminders) {
        if ('Notification' in window && Notification.permission === 'granted') {
            const count = urgentReminders.length;
            const title = `Payment Reminder Alert`;
            const body = `You have ${count} urgent payment reminder${count > 1 ? 's' : ''} that need attention.`;
            
            const notification = new Notification(title, {
                body: body,
                icon: '/favicon.ico',
                badge: '/favicon.ico',
                tag: 'reminder-alert',
                requireInteraction: true
            });
            
            notification.onclick = function() {
                window.focus();
                notification.close();
            };
        }
    }
    
    showInPageAlert(urgentReminders) {
        // Remove existing alerts
        $('.reminder-alert').remove();
        
        const alertHtml = `
            <div class="reminder-alert alert alert-warning alert-dismissible fade show position-fixed" 
                 style="top: 20px; right: 20px; z-index: 9999; max-width: 400px; box-shadow: 0 4px 12px rgba(0,0,0,0.15);">
                <div class="d-flex align-items-center">
                    <i class="fas fa-bell text-warning me-2"></i>
                    <div class="flex-grow-1">
                        <h6 class="alert-heading mb-1">Payment Reminders</h6>
                        <p class="mb-2">You have ${urgentReminders.length} urgent reminder${urgentReminders.length > 1 ? 's' : ''}!</p>
                        <div class="small">
                            ${urgentReminders.slice(0, 3).map(r => 
                                `<div class="mb-1">
                                    <strong>${r.clientName}</strong> - $${r.amount.toLocaleString()}
                                    <span class="badge badge-sm ${r.priority === 'Critical' ? 'badge-danger' : 'badge-warning'} ms-2">
                                        ${r.priority}
                                    </span>
                                </div>`
                            ).join('')}
                            ${urgentReminders.length > 3 ? `<div class="text-muted">... and ${urgentReminders.length - 3} more</div>` : ''}
                        </div>
                    </div>
                </div>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                <div class="mt-3">
                    <a href="/Reminder" class="btn btn-sm btn-primary me-2">
                        <i class="fas fa-eye me-1"></i>View All
                    </a>
                    <button type="button" class="btn btn-sm btn-outline-secondary" onclick="dismissReminderAlert()">
                        Dismiss
                    </button>
                </div>
            </div>
        `;
        
        $('body').append(alertHtml);
        
        // Auto-dismiss after 30 seconds
        setTimeout(() => {
            $('.reminder-alert').fadeOut();
        }, 30000);
    }
    
    updatePageTitle(urgentCount) {
        if (urgentCount > 0) {
            const originalTitle = document.title.replace(/^\[!\d+\]\s*/, '');
            document.title = `[!${urgentCount}] ${originalTitle}`;
        } else {
            document.title = document.title.replace(/^\[!\d+\]\s*/, '');
        }
    }
}

// Global function to dismiss reminder alerts
function dismissReminderAlert() {
    $('.reminder-alert').fadeOut();
}

// Initialize the reminder notification system when the page loads
$(document).ready(function() {
    window.reminderNotifications = new ReminderNotificationSystem();
});

// Export for use in other scripts
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ReminderNotificationSystem;
}
