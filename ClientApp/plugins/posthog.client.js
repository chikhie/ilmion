import { defineNuxtPlugin } from '#app'
import posthog from 'posthog-js'

export default defineNuxtPlugin(nuxtApp => {
    const runtimeConfig = useRuntimeConfig();
    const posthogKey = runtimeConfig.public.posthogPublicKey;

    if (!posthogKey) {
        console.error('PostHog Public Key is missing in RuntimeConfig!');
        return;
    }

    // Disable in development
    if (process.env.NODE_ENV === 'development' || window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1') {
        console.log('PostHog disabled in development');
        return;
    }

    const posthogClient = posthog.init(posthogKey, {
        api_host: window.location.origin + '/ingest',
        defaults: runtimeConfig.public.posthogDefaults,
        loaded: (posthog) => {
            if (import.meta.env.MODE === 'development') posthog.debug();
        }
    })

    return {
        provide: {
            posthog: () => posthogClient
        }
    }
})