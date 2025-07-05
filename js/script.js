document.addEventListener("DOMContentLoaded", () => {
  const links = document.querySelectorAll("nav a");
  const content = document.getElementById("content");
  const themeToggle = document.getElementById("themeToggle");

  function loadPage(page) {
    content.style.opacity = 0;

    fetch(`pages/${page}.html`)
      .then(res => {
        if (!res.ok) throw new Error(`Failed to load ${page}.html`);
        return res.text();
      })
      .then(html => {
        setTimeout(() => {
          content.innerHTML = html;
          content.style.opacity = 1;

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
        }, 200);
      })
      .catch(err => {
        content.innerHTML = `<p>Error loading page: ${err.message}</p>`;
        content.style.opacity = 1;
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
      alert(`Thanks for your order! Total: £${cartTotalSpan.textContent}`);
      cart = [];
      updateCartUI();
      cartPanel.classList.remove("open");
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

    cartItemsContainer.innerHTML = '';
    let total = 0;
    let count = 0;

    if (cart.length === 0) {
      cartItemsContainer.innerHTML = '<p>Your cart is empty.</p>';
    } else {
      cart.forEach((item, idx) => {
        total += item.price * item.qty;
        count += item.qty;

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
    }

    cartCountSpan.textContent = count;
    cartTotalSpan.textContent = total.toFixed(2);
    saveCart();
  }

  loadCart();
  updateCartUI();
  loadPage("home");
});