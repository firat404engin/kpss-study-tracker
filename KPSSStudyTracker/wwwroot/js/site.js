// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Dark mode toggle
(function () {
    const key = 'theme';
    const btn = document.getElementById('themeToggle');
    const root = document.documentElement;
    const current = localStorage.getItem(key) || 'light';
    if (current === 'dark') {
        root.setAttribute('data-bs-theme', 'dark');
        document.querySelector('.theme-light')?.classList.add('d-none');
        document.querySelector('.theme-dark')?.classList.remove('d-none');
    }
    if (btn) {
        btn.addEventListener('click', function () {
            const isDark = root.getAttribute('data-bs-theme') === 'dark';
            if (isDark) {
                root.removeAttribute('data-bs-theme');
                localStorage.setItem(key, 'light');
                document.querySelector('.theme-light')?.classList.remove('d-none');
                document.querySelector('.theme-dark')?.classList.add('d-none');
            } else {
                root.setAttribute('data-bs-theme', 'dark');
                localStorage.setItem(key, 'dark');
                document.querySelector('.theme-light')?.classList.add('d-none');
                document.querySelector('.theme-dark')?.classList.remove('d-none');
            }
        });
    }
})();
// Write your JavaScript code.
