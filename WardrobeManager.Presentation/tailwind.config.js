module.exports = {
    content: [
        "./Pages/*.razor",
        "./Pages/**/*.razor",
        "./Components/*.razor",
        "./Components/**/*.razor",
        "./wwwroot/index.html",
    ],
    theme: {
        extend: {},
    },
    plugins: [
        require('daisyui'),
    ],
}

