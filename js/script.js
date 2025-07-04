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

          // Highlight nav link
          links.forEach(link => {
            link.classList.remove("active");
            if (link.dataset.page === page) {
              link.classList.add("active");
            }
          });
        }, 300);
      })
      .catch(err => {
        content.innerHTML = `<p>Error loading page: ${err.message}</p>`;
        content.style.opacity = 1;
      });
  }

  links.forEach(link => {
    link.addEventListener("click", e => {
      e.preventDefault();
      const page = link.dataset.page;
      loadPage(page);
    });
  });

  themeToggle.addEventListener("click", () => {
    const html = document.documentElement;
    const current = html.getAttribute("data-theme");
    const newTheme = current === "dark" ? "light" : "dark";
    html.setAttribute("data-theme", newTheme);
    themeToggle.textContent = newTheme === "dark" ? "🌙" : "🌞";
  });

  // Load home page on start
  loadPage("home");
});
