const API_BASE_URL = 'http://localhost:5087/api'; // Backend URL'sini kontrol edin

const App = {
    init() {
        console.log("WordFlow App Initialized");
        App.initTheme();
        App.initLexiBot();
    },
    
    initTheme() {
        const theme = localStorage.getItem('theme');
        if (theme === 'dark') {
            document.documentElement.setAttribute('data-theme', 'dark');
        }

        // Yüzen Tema Değiştirme Butonu
        if (!document.getElementById('themeToggleBtn')) {
            const btn = document.createElement('button');
            btn.id = 'themeToggleBtn';
            btn.className = 'theme-toggle-btn';
            btn.innerHTML = App.getThemeIcon(theme === 'dark');
            btn.onclick = App.toggleTheme;
            document.body.appendChild(btn);
        }

        // Yüzen Ses Aç/Kapat Butonu
        if (!document.getElementById('soundToggleBtn')) {
            const sBtn = document.createElement('button');
            sBtn.id = 'soundToggleBtn';
            sBtn.className = 'theme-toggle-btn';
            sBtn.style.bottom = '84px'; // Temanın hemen üstünde
            const isSoundOn = localStorage.getItem('uiSound') !== 'off';
            sBtn.innerHTML = App.getSoundIcon(isSoundOn);
            sBtn.onclick = App.toggleUISound;
            document.body.appendChild(sBtn);
        }

        // Tüm sayfalarda buton ve linklere tıklandığında ses çal
        document.body.addEventListener('click', (e) => {
            const el = e.target.closest('button, a');
            // Sadece theme ve sound butonları hariç her yere tıklamada çalabiliriz
            // ya da onlara da çalabiliriz.
            if (el) {
                App.playClickSound();
            }
        });
    },

    getThemeIcon(isDark) {
        if (isDark) {
            // Güneş (Açık temaya geçiş için)
            return `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="5"></circle><line x1="12" y1="1" x2="12" y2="3"></line><line x1="12" y1="21" x2="12" y2="23"></line><line x1="4.22" y1="4.22" x2="5.64" y2="5.64"></line><line x1="18.36" y1="18.36" x2="19.78" y2="19.78"></line><line x1="1" y1="12" x2="3" y2="12"></line><line x1="21" y1="12" x2="23" y2="12"></line><line x1="4.22" y1="19.78" x2="5.64" y2="18.36"></line><line x1="18.36" y1="5.64" x2="19.78" y2="4.22"></line></svg>`;
        } else {
            // Ay (Karanlık temaya geçiş için)
            return `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"></path></svg>`;
        }
    },

    getSoundIcon(isOn) {
        if (isOn) {
            // Volume On
            return `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polygon points="11 5 6 9 2 9 2 15 6 15 11 19 11 5"></polygon><path d="M19.07 4.93a10 10 0 0 1 0 14.14M15.54 8.46a5 5 0 0 1 0 7.07"></path></svg>`;
        } else {
            // Volume Muted
            return `<svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><polygon points="11 5 6 9 2 9 2 15 6 15 11 19 11 5"></polygon><line x1="23" y1="9" x2="17" y2="15"></line><line x1="17" y1="9" x2="23" y2="15"></line></svg>`;
        }
    },

    toggleUISound(event) {
        if(event) event.stopPropagation(); // Butona basıldığında playClickSound tetiklenmesin diye
        const isSoundOn = localStorage.getItem('uiSound') !== 'off';
        const sBtn = document.getElementById('soundToggleBtn');
        
        if (isSoundOn) {
            localStorage.setItem('uiSound', 'off');
            if (sBtn) sBtn.innerHTML = App.getSoundIcon(false);
        } else {
            localStorage.setItem('uiSound', 'on');
            if (sBtn) sBtn.innerHTML = App.getSoundIcon(true);
            App.playClickSound(); // Sesi açtığında örnek bir ses çalsın
        }
    },

    playClickSound() {
        if (localStorage.getItem('uiSound') === 'off') return;
        
        try {
            // Herhangi bir dış MP3 dosyasına gerek kalmadan Web Audio API ile 'pop' sesi üretme
            const AudioContext = window.AudioContext || window.webkitAudioContext;
            if (!AudioContext) return;
            
            const ctx = new AudioContext();
            const osc = ctx.createOscillator();
            const gain = ctx.createGain();
            
            osc.connect(gain);
            gain.connect(ctx.destination);
            
            osc.type = 'sine'; // Yumuşak bir ses tonu
            
            // Pop / Click efekti için frekans ve ses seviyesi ayarları
            osc.frequency.setValueAtTime(800, ctx.currentTime);
            osc.frequency.exponentialRampToValueAtTime(300, ctx.currentTime + 0.05);
            
            gain.gain.setValueAtTime(0.3, ctx.currentTime);
            gain.gain.exponentialRampToValueAtTime(0.01, ctx.currentTime + 0.05);
            
            osc.start(ctx.currentTime);
            osc.stop(ctx.currentTime + 0.05);
        } catch (e) {
            console.error("Ses çalınamadı:", e);
        }
    },

    toggleTheme() {
        const currentTheme = document.documentElement.getAttribute('data-theme');
        const btn = document.getElementById('themeToggleBtn');
        
        if (currentTheme === 'dark') {
            document.documentElement.removeAttribute('data-theme');
            localStorage.setItem('theme', 'light');
            if (btn) btn.innerHTML = App.getThemeIcon(false);
        } else {
            document.documentElement.setAttribute('data-theme', 'dark');
            localStorage.setItem('theme', 'dark');
            if (btn) btn.innerHTML = App.getThemeIcon(true);
        }
    },
    
    // API İstekleri için yardımcı fonksiyon (Token ekler)
    async apiCall(endpoint, method = 'GET', body = null) {
        const headers = {
            'Content-Type': 'application/json'
        };
        
        const token = localStorage.getItem('token');
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        const config = {
            method,
            headers
        };

        if (body) {
            config.body = JSON.stringify(body);
        }

        try {
            const response = await fetch(`${API_BASE_URL}${endpoint}`, config);
            let data;
            const text = await response.text();
            try { data = JSON.parse(text); } catch { data = null; }
            
            if (!response.ok) {
                // Eğer ASP.NET Core doğrulama hatası ise errors objesinde detaylar yazar
                let errMsg = data?.message || data?.title || 'Bir hata oluştu';
                if (data && data.errors) {
                    const firstErrorKey = Object.keys(data.errors)[0];
                    if (data.errors[firstErrorKey].length > 0) {
                        errMsg = data.errors[firstErrorKey][0];
                    }
                }
                throw new Error(`${errMsg} (Hata Kodu: ${response.status})`);
            }
            return data;
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    },

    setToken(token) {
        localStorage.setItem('token', token);
    },

    getToken() {
        return localStorage.getItem('token');
    },

    logout() {
        localStorage.removeItem('token');
        window.location.href = 'index.html';
    },

    // Kelimeyi sesli okuma (Text-to-Speech) özelliği
    speakWord(text, event) {
        // Eğer bir butona tıklandıysa event'in yayılmasını (tıklama) engelle
        if(event) event.stopPropagation();
        
        if (!('speechSynthesis' in window)) {
            App.showLexiMessage("Tarayıcınız sesli okuma özelliğini desteklemiyor.", true);
            return;
        }
        
        // Önceki konuşmaları durdur
        window.speechSynthesis.cancel();
        
        const utterance = new SpeechSynthesisUtterance(text);
        utterance.lang = 'en-US'; // İngilizce okutuyoruz
        utterance.rate = 0.85;    // Biraz daha yavaş ve net anlaşılması için
        utterance.pitch = 1.0;
        
        // Lexi konuşmaya başladığında
        utterance.onstart = () => {
            App.showLexiMessage(`🔊 "${text}"`, false);
            const img = document.getElementById('lexiBotImg');
            if(img) img.classList.add('lexi-talking');
        };

        // Konuşma bittiğinde
        utterance.onend = () => {
            const img = document.getElementById('lexiBotImg');
            if(img) img.classList.remove('lexi-talking');
        };

        window.speechSynthesis.speak(utterance);
    },

    showError(elementId, message) {
        // Eski kırmızı kutu uyarıları yerine Lexi üzerinden mesaj verelim
        App.showLexiMessage(message, true);
    },

    showSuccess(elementId, message) {
        // Eski yeşil kutu uyarıları yerine Lexi üzerinden mesaj verelim
        App.showLexiMessage(message, false);
    },

    initLexiBot() {
        // LexiBot global DOM elementini oluştur
        if (document.getElementById('lexiBotContainer')) return;

        const container = document.createElement('div');
        container.id = 'lexiBotContainer';
        container.className = 'lexi-bot-container';
        
        container.innerHTML = `
            <div id="lexiBotBubble" class="lexi-bot-bubble">Merhaba, ben Lexi! Sana nasıl yardımcı olabilirim? 🦊</div>
            <img id="lexiBotImg" src="img/lexi.png" alt="Lexi" class="lexi-bot-img">
        `;
        document.body.appendChild(container);

        // Easter Egg (Gizli Sürprizler)
        let clickCount = 0;
        const lexiImg = document.getElementById('lexiBotImg');
        const bubble = document.getElementById('lexiBotBubble');

        lexiImg.addEventListener('click', () => {
            clickCount++;
            App.playClickSound();
            
            // Etrafında dönme animasyonu ekle
            lexiImg.classList.remove('lexi-spin');
            void lexiImg.offsetWidth; // reflow
            lexiImg.classList.add('lexi-spin');

            container.classList.add('show');

            if (clickCount === 1) {
                bubble.innerText = "Heey! Gıdıklanıyorum! 😄";
            } else if (clickCount === 3) {
                bubble.innerText = "Kelime çalışmayı bıraktın benimle oynuyorsun bakıyorum! 🦊";
                lexiImg.src = 'img/lexi_search.png';
            } else if (clickCount === 5) {
                bubble.innerText = "Tamam tamam, sen kazandın! Sana kocaman bir Lexi kalbi! ❤️";
                lexiImg.src = 'img/lexi_fail.png'; // Kalp tutan lexi
                clickCount = 0; // Sıfırla
            }

            // Mesajı bir süre sonra gizle
            clearTimeout(App.lexiTimer);
            App.lexiTimer = setTimeout(() => {
                container.classList.remove('show');
                lexiImg.src = 'img/lexi.png'; // Eski haline dön
            }, 4000);
        });
    },

    showLexiMessage(message, isError = false) {
        const container = document.getElementById('lexiBotContainer');
        const bubble = document.getElementById('lexiBotBubble');
        const img = document.getElementById('lexiBotImg');
        
        if (!container) return;

        bubble.innerText = message;
        if (isError) {
            bubble.style.border = '2px solid var(--danger)';
            bubble.style.color = 'var(--danger)';
            img.src = 'img/lexi_search.png'; // Şaşkın/Arayan lexi
        } else {
            bubble.style.border = '2px solid var(--success)';
            bubble.style.color = 'var(--success)';
            img.src = 'img/lexi_success.png'; // Sevinen lexi
        }

        container.classList.add('show');
        App.playClickSound();

        clearTimeout(App.lexiTimer);
        App.lexiTimer = setTimeout(() => {
            container.classList.remove('show');
            // Stilleri sıfırla
            setTimeout(() => {
                bubble.style.border = '1px solid var(--border-color)';
                bubble.style.color = 'var(--text-dark)';
                img.src = 'img/lexi.png';
            }, 300);
        }, 5000);
    },

    startOnboarding() {
        if (localStorage.getItem('onboardingDone') === 'true') return;

        // Overlay oluştur
        const overlay = document.createElement('div');
        overlay.id = 'onboardingOverlay';
        overlay.className = 'onboarding-overlay';
        
        overlay.innerHTML = `
            <div class="onboarding-lexi-center" id="onboardingCenter">
                <img src="img/lexi.png" alt="Lexi">
                <div class="bubble" id="onboardingBubble">Merhaba! Ben Lexi. Öğrenme yolculuğunda sana eşlik edeceğim! 🦊</div>
                <button class="btn btn-primary" id="onboardingNextBtn" style="border-radius: 24px; padding: 12px 40px; margin-top: 24px;">Devam Et</button>
            </div>
        `;
        document.body.appendChild(overlay);

        let step = 0;
        const steps = [
            { text: "Burada her gün senin seviyene uygun yepyeni bir kelime üreteceğim.", targetSelector: ".word-of-day-card" },
            { text: "Burası senin kelime kütüphanen. Öğrendiğin tüm kelimeleri buradan takip edebilirsin.", targetSelector: ".nav-menu li:nth-child(2) .nav-link" },
            { text: "Kendini hazır hissettiğinde, öğrendiklerini test etmek için buradan Quiz'e katılabilirsin.", targetSelector: ".nav-menu li:nth-child(3) .nav-link" },
            { text: "Ya da eğlenerek öğrenmek istersen oyunlar tam sana göre!", targetSelector: ".nav-menu li:nth-child(4) .nav-link" },
            { text: "Hazırsan başlayalım! Günün kelimesi seni bekliyor.", targetSelector: null }
        ];

        let currentHighlight = null;

        const nextStep = () => {
            if (currentHighlight) {
                currentHighlight.classList.remove('onboarding-highlight');
                // Sidebar'dan z-index düzeltmesini kaldır
                const sidebar = currentHighlight.closest('.sidebar');
                if (sidebar) sidebar.style.zIndex = '10';
                currentHighlight = null;
            }

            if (step >= steps.length) {
                // Bitir
                overlay.classList.remove('show');
                localStorage.setItem('onboardingDone', 'true');
                setTimeout(() => overlay.remove(), 500);
                return;
            }

            const current = steps[step];
            document.getElementById('onboardingBubble').innerText = current.text;
            
            if (current.targetSelector) {
                currentHighlight = document.querySelector(current.targetSelector);
            }

            if (currentHighlight) {
                currentHighlight.classList.add('onboarding-highlight');
                // Eğer highlight edilen obje sidebar içindeyse sidebar'ın z-index'ini geçici olarak yükselt (overlay üstüne çıksın)
                const sidebar = currentHighlight.closest('.sidebar');
                if (sidebar) sidebar.style.zIndex = '9999';
            }

            if(step === steps.length - 1) {
                document.getElementById('onboardingNextBtn').innerText = "Başla!";
            }
            
            step++;
            App.playClickSound();
        };

        document.getElementById('onboardingNextBtn').addEventListener('click', nextStep);

        // Biraz gecikmeli göster
        setTimeout(() => overlay.classList.add('show'), 500);
    }
};

document.addEventListener('DOMContentLoaded', App.init);
