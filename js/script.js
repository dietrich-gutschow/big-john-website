document.addEventListener("DOMContentLoaded", () => {
    const links = document.querySelectorAll("nav a");
    const content = document.getElementById("content");
    const themeToggle = document.getElementById("themeToggle");

    function loadPage(page) {
        const content = document.getElementById("content"); // main panel
        const pageContainer = document.getElementById("page-inner"); // just the swappable content

        pageContainer.style.opacity = 0;

        fetch(`pages/${page}.html`)
            .then(res => {
                if (!res.ok) throw new Error(`Failed to load ${page}.html`);
                return res.text();
            })
            .then(html => {
                setTimeout(() => {
                    pageContainer.innerHTML = html;
                    pageContainer.style.opacity = 1;

                    links.forEach(link => {
                        link.classList.remove("active");
                        if (link.dataset.page === page) {
                            link.classList.add("active");
                        }
                    });

                    observeScrollElements();

                    if (page === "menu") {
                        setupCartEvents();
                    }

                    if (page === "bigjohntv") {
                        const videoIDs = [
                            "Jv2NT_A0yYo",
                            "XX9VE85HjDM",
                            "TRfToys9S3o",
                            "JZ_2ig2kpWw",
                            "OV75xgpAfl8"   // BIG JOHN'S VIDEOS
                        ];
                        const randomID = videoIDs[Math.floor(Math.random() * videoIDs.length)];
                        const player = document.getElementById("johnPlayer");
                        if (player) {
                            player.src = `https://www.youtube.com/embed/${randomID}?autoplay=1&mute=1&rel=0`;
                        }
                    }
                }, 100);
            })
            .catch(err => {
                pageContainer.innerHTML = `<p>Error loading page: ${err.message}</p>`;
                pageContainer.style.opacity = 1;
            });
    }


    links.forEach(link => {
        link.addEventListener("click", e => {
            e.preventDefault();
            loadPage(link.dataset.page);
        });
    });

    themeToggle.addEventListener("click", () => {
        const html = document.documentElement;
        const current = html.getAttribute("data-theme");
        const newTheme = current === "dark" ? "light" : "dark";
        html.setAttribute("data-theme", newTheme);
        themeToggle.textContent = newTheme === "dark" ? "🌙" : "🌞";
    });

    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
            }
        });
    }, {
        threshold: 0.1
    });

    function observeScrollElements() {
        const elements = document.querySelectorAll('.scroll-fade');
        elements.forEach(el => observer.observe(el));
    }

    function setupCartEvents() {
        const cartButton = document.getElementById("cartButton");
        const cartPanel = document.getElementById("cartPanel");
        const closeCartBtn = document.getElementById("closeCart");
        const cartItemsContainer = document.getElementById("cartItems");
        const cartCountSpan = document.getElementById("cartCount");
        const cartTotalSpan = document.getElementById("cartTotal");
        const buyButton = document.getElementById("buyButton");

        if (!cartButton || !cartPanel || !closeCartBtn) return;

        cartButton.addEventListener("click", () => {
            cartPanel.classList.add("open");
        });

        closeCartBtn.addEventListener("click", () => {
            cartPanel.classList.remove("open");
        });

        cartItemsContainer?.addEventListener("click", (e) => {
            const idx = parseInt(e.target.dataset.index, 10);
            if (e.target.classList.contains("qty-increase")) {
                cart[idx].qty++;
            } else if (e.target.classList.contains("qty-decrease")) {
                cart[idx].qty = Math.max(1, cart[idx].qty - 1);
            } else if (e.target.classList.contains("remove-item")) {
                cart.splice(idx, 1);
            }
            updateCartUI();
        });

        buyButton?.addEventListener("click", () => {
            if (cart.length === 0) {
                alert("Your cart is empty!");
                return;
            }

            const total = parseFloat(document.getElementById("cartTotal").textContent);

            if (total >= 20 && total < 25) {
                const confirmUpsell = confirm(
                    "Spend £25 to get free prawn crackers!\n\nPress OK to continue with payment.\nPress Cancel to go back and add more items."
                );

                if (!confirmUpsell) {
                    // Cancelled: let user add more items
                    document.getElementById("cartPanel").classList.remove("open");
                    return;
                }
            }

            alert(`Thanks for your order! Total: £${total.toFixed(2)}`);
            cart = [];
            updateCartUI();
            document.getElementById("cartPanel").classList.remove("open");
        });


        document.querySelectorAll(".add-to-cart").forEach((btn) => {
            btn.addEventListener("click", () => {
                addToCart(btn.dataset.name, btn.dataset.price);
            });
        });
    }

    let cart = [];

    function addToCart(name, price) {
        const index = cart.findIndex(item => item.name === name);
        if (index > -1) {
            cart[index].qty += 1;
        } else {
            cart.push({ name, price: parseFloat(price), qty: 1 });
        }
        updateCartUI();
    }

    function saveCart() {
        localStorage.setItem("bigJohnCart", JSON.stringify(cart));
    }

    function loadCart() {
        const stored = localStorage.getItem("bigJohnCart");
        if (stored) {
            cart = JSON.parse(stored);
        }
    }

    function updateCartUI() {
        const cartItemsContainer = document.getElementById("cartItems");
        const cartCountSpan = document.getElementById("cartCount");
        const cartTotalSpan = document.getElementById("cartTotal");

        if (!cartItemsContainer) return;

        // Remove free crackers if present
        cart = cart.filter(item => item.name !== "Free Prawn Crackers");

        let total = 0;
        let count = 0;

        // First pass: calculate total
        cart.forEach(item => {
            total += item.price * item.qty;
            count += item.qty;
        });

        // Add free crackers if total >= 25
        if (total >= 25) {
            cart.push({ name: "Free Prawn Crackers", price: 0, qty: 1 });
            // Recalculate count (but not total since price = 0)
            count += 1;
        }

        // Render cart UI
        cartItemsContainer.innerHTML = '';
        cart.forEach((item, idx) => {
            const itemEl = document.createElement('div');
            itemEl.className = 'cart-item';
            itemEl.innerHTML = `
      <div class="cart-item-name">${item.name}</div>
      <div class="cart-item-controls">
        <button class="qty-decrease" data-index="${idx}">−</button>
        <div class="cart-item-qty">${item.qty}</div>
        <button class="qty-increase" data-index="${idx}">+</button>
        <button class="remove-item" data-index="${idx}">✖</button>
      </div>
    `;
            cartItemsContainer.appendChild(itemEl);
        });

        cartCountSpan.textContent = count;
        cartTotalSpan.textContent = total.toFixed(2);
        saveCart();
    }



    function spawnNoodles(count = 20) {
        const container = document.querySelector(".noodle-container");
        if (!container) return;

        const noodleEmoji = ['🍜', '🥢', '🥡'];

        for (let i = 0; i < count; i++) {
            const noodle = document.createElement("div");
            noodle.classList.add("noodle");

            noodle.textContent = noodleEmoji[Math.floor(Math.random() * noodleEmoji.length)];

            // Start just below the screen
            noodle.style.left = `${Math.random() * 100}%`;
            noodle.style.top = `${100 + Math.random() * 20}vh`; // 100vh–120vh

            // Random animation duration and delay
            const duration = 15 + Math.random() * 15;
            const delay = Math.random() * 10;

            noodle.style.animationDuration = `${duration}s`;
            noodle.style.animationDelay = `${delay}s`;

            container.appendChild(noodle);
        }
    }

    loadCart();
    updateCartUI();
    loadPage("home");
    spawnNoodles(25); // Adjust count as needed
});