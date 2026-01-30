import { defineNuxtPlugin } from '#app'
import posthog from 'posthog-js'

export default defineNuxtPlugin(nuxtApp => {
    const runtimeConfig = useRuntimeConfig();
    const posthogKey = runtimeConfig.public.posthogPublicKey;

    if (!posthogKey) {
        console.error('PostHog Public Key is missing in RuntimeConfig!');
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