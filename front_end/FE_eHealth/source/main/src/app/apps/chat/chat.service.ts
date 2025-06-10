// src/app/services/chat.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserEnviroment } from 'environments/environment';

export interface ChatUser {
    id: number;
    fullName: string;
    avatarUrl?: string;
    isOnline: boolean;
}

export interface MessageDto {
    id: number;
    senderId: number;
    receiverId: number;
    content: string;
    sentAt: string;   // Angular sẽ nhận ISO string, chuyển sang Date khi hiển thị
    isRead: boolean;
    readAt?: string;  // ISO string
}

export interface SendMessageDto {
    senderId: number;
    receiverId: number;
    content: string;
}

@Injectable({
    providedIn: 'root'
})
export class ChatService {
    //   private apiUrl = 'https://localhost:5175/api/Chat';
    private apiUrl = `${UserEnviroment.apiUrl}/chat`;

    constructor(private http: HttpClient) { }

    // Lấy danh sách user (contacts)
    getAllUsers(excludeId: number): Observable<ChatUser[]> {
        const params = new HttpParams().set('excludeId', excludeId.toString());
        return this.http.get<ChatUser[]>(`${this.apiUrl}/users`, { params });
    }

    // Lấy lịch sử tin nhắn giữa 2 user
    getMessages(user1: number, user2: number): Observable<MessageDto[]> {
        const params = new HttpParams()
            .set('user1', user1.toString())
            .set('user2', user2.toString());
        return this.http.get<MessageDto[]>(`${this.apiUrl}/messages`, { params });
    }

    // Gửi tin nhắn mới
    sendMessage(dto: SendMessageDto): Observable<MessageDto> {
        return this.http.post<MessageDto>(`${this.apiUrl}/messages`, dto);
    }

    // (Tuỳ chọn) Đánh dấu message đã đọc
    markAsRead(messageId: number): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/messages/${messageId}/read`, {});
    }
}
