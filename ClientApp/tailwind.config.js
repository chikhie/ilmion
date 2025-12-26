/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./components/**/*.{js,vue,ts}",
    "./layouts/**/*.vue",
    "./pages/**/*.vue",
    "./plugins/**/*.{js,ts}",
    "./app.vue",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#f0f9ff',
          100: '#e0f2fe',
          200: '#bae6fd',
          300: '#7dd3fc',
          400: '#38bdf8',
          500: '#0ea5e9',
          600: '#0284c7',
          700: '#0369a1',
          800: '#075985',
          900: '#0c4a6e',
        },
        brand: {
          dark: '#082540',
          wood: '#1a3c5a',
          parchment: '#DDE1E7',
          'parchment-light': '#f5f7fa',
          gold: '#C39712',
        },
      },
      fontFamily: {
        'serif-title': ['Merriweather', 'Libre Baskerville', 'serif'],
        'sans-body': ['Inter', 'Source Sans 3', 'sans-serif'],
        'arabic': ['Amiri', 'serif'],
      },
    },
  },
  plugins: [],
}

