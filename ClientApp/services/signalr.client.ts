import { HubConnectionBuilder, HubConnection, LogLevel } from '@microsoft/signalr'

type HubEventHandler = (...args: any[]) => void

export class SignalRClient {
    private connection: HubConnection
    private connected = false

    constructor(hubUrl: string, accessToken?: string) {
        let builder = new HubConnectionBuilder()
            .withUrl(hubUrl, {
                accessTokenFactory: accessToken ? () => accessToken : undefined
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Information)

        this.connection = builder.build()

        this.connection.onclose((error) => {
            this.connected = false
            console.log('SignalR Connection closed', error)
        })

        this.connection.onreconnecting((error) => {
            console.log('SignalR Reconnecting...', error)
        })

        this.connection.onreconnected((connectionId) => {
            this.connected = true
            console.log('SignalR Reconnected', connectionId)
        })
    }

    async start(): Promise<void> {
        if (this.connection.state === 'Connected') {
            this.connected = true
            return
        }
        if (this.connection.state === 'Connecting' || this.connection.state === 'Reconnecting') {
            // Wait for it? For now, we just let it finish or throw.
            // Ideally we return a promise that resolves when connected.
            // But simplistic approach:
            return
        }
        try {
            await this.connection.start()
            this.connected = true
            console.log('SignalR Connected')
        } catch (err) {
            console.error('SignalR Connection Error', err)
            this.connected = false
            throw err
        }
    }

    async stop(): Promise<void> {
        if (!this.connected) return
        await this.connection.stop()
        this.connected = false
    }

    on(methodName: string, newMethod: HubEventHandler): void {
        this.connection.on(methodName, newMethod)
    }

    off(methodName: string, method?: HubEventHandler): void {
        if (method) {
            this.connection.off(methodName, method)
        } else {
            this.connection.off(methodName)
        }
    }

    async invoke<T>(methodName: string, ...args: any[]): Promise<T> {
        if (!this.connected) {
            throw new Error("SignalR not connected")
        }
        return await this.connection.invoke(methodName, ...args)
    }

    async send(methodName: string, ...args: any[]): Promise<void> {
        if (!this.connected) {
            throw new Error("SignalR not connected")
        }
        return await this.connection.send(methodName, ...args)
    }
}
