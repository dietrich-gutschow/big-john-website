document.addEventListener("DOMContentLoaded", () => {
  const links = document.querySelectorAll("nav a");
  const content = document.getElementById("content");
  const themeToggle = document.getElementById("themeToggle");

  function loadPage(page) {
    content.style.opacity = 0;
    fetch(`pages/${page}.html`)
      .then(res => res.text())
      .then(html => {
        setTimeout(() => {
          content.innerHTML = html;
          content.style.opacity = 1;
        }, 300);
      });
  }

  links.forEach(link => {
    link.addEventListener("click", (e) => {
      e.preventDefault();
      const page = link.getAttribute("data-page");
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

  // Load home page by default
  loadPage("home");
});
