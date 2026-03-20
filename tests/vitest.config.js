import { defineConfig } from 'vitest/config'

export default defineConfig({
    test: {
        globals: true,
        include: ['dist/Program.test.js'],
        exclude: ['**/node_modules/**', '**/dist/fable_modules/**'],
    },
})
