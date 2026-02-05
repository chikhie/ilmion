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

    const posthogClient = {
        capture: (...args) => {
            // Placeholder: Could queue events here if critical, 
            // but for now we just rely on the real instance loading.
            if (window.posthog) {
                window.posthog.capture(...args);
            }
        }
    };

    // Lazy load PostHog
    if (import.meta.client) {
        import('posthog-js').then(({ default: posthog }) => {
            posthog.init(posthogKey, {
                api_host: window.location.origin + '/ingest',
                defaults: runtimeConfig.public.posthogDefaults,
                loaded: (ph) => {
                    if (import.meta.env.MODE === 'development') ph.debug();
                    // Expose globally if needed or just let it run
                }
            })
        }).catch(err => console.error('Failed to load PostHog', err));
    }

    return {
        provide: {
            posthog: () => posthogClient
        }
    }
})