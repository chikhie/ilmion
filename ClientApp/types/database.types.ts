export type Json =
    | string
    | number
    | boolean
    | null
    | { [key: string]: Json | undefined }
    | Json[]

export interface Database {
    public: {
        Tables: {
            parties: {
                Row: {
                    id: string
                    created_at: string
                    code: string
                    game_id: string
                    host_username: string
                    status: string
                    current_question_index: number
                }
                Insert: {
                    id?: string
                    created_at?: string
                    code: string
                    game_id: string
                    host_username: string
                    status: string
                    current_question_index?: number
                }
                Update: {
                    id?: string
                    created_at?: string
                    code?: string
                    game_id?: string
                    host_username?: string
                    status?: string
                    current_question_index?: number
                }
                Relationships: [
                    {
                        foreignKeyName: "parties_game_id_fkey"
                        columns: ["game_id"]
                        referencedRelation: "games"
                        referencedColumns: ["id"]
                    }
                ]
            }
            party_players: {
                Row: {
                    id: string
                    created_at: string
                    party_id: string
                    username: string
                    is_ready: boolean
                    score: number
                    has_answered: boolean
                }
                Insert: {
                    id?: string
                    created_at?: string
                    party_id: string
                    username: string
                    is_ready?: boolean
                    score?: number
                    has_answered?: boolean
                }
                Update: {
                    id?: string
                    created_at?: string
                    party_id?: string
                    username?: string
                    is_ready?: boolean
                    score?: number
                    has_answered?: boolean
                }
                Relationships: [
                    {
                        foreignKeyName: "party_players_party_id_fkey"
                        columns: ["party_id"]
                        referencedRelation: "parties"
                        referencedColumns: ["id"]
                    }
                ]
            }
        }
        Views: {
            [_ in never]: never
        }
        Functions: {
            increment_question_index: {
                Args: {
                    party_id_param: string
                }
                Returns: void
            }
        }
        Enums: {
            [_ in never]: never
        }
    }
}
