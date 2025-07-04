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

  function spawnNoodles() {
    const container = document.querySelector('.noodle-container');
    setInterval(() => {
      const noodle = document.createElement('div');
      noodle.className = 'noodle';
      noodle.textContent = '🍜';
      noodle.style.left = Math.random() * 100 + 'vw';
      noodle.style.animationDuration = (5 + Math.random() * 5) + 's';
      noodle.style.fontSize = (1 + Math.random() * 2) + 'rem';
      container.appendChild(noodle);

      setTimeout(() => noodle.remove(), 10000);
    }, 400);
  }

  spawnNoodles();
  loadPage("home");
});
