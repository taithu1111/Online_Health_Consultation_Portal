import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserEnviroment } from 'environments/environment';
import { UserWithProfile } from '@core/models/userWithProfile';

export interface User {
    id: number;
    imageUrl: string;
    email: string;
    fullName: string;
    gender?: string;
    role: string;
    dateOfBirth?: string;
    bloodType?: string;
    phoneNumber?: string;
    address?: string;
    bio?: string;
    specialization?: string; // doctor field
    experienceYears?: number; // doctor field
    languages?: string;
    consultationFee?: number; // doctor field
}

export interface UpdateUserProfileDto {
    fullName?: string;
    gender?: string;
    imageUrl?: string;
    dateOfBirth?: string;
    bloodType?: string;
    phone?: string;
    address?: string;
    bio?: string;
    specialization?: number[]; // doctor field
    experienceYears?: number; // doctor field
    languages?: string;
    consultationFee?: number; // doctor field
}

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private apiUrl = `${UserEnviroment.apiUrl}/users`; // Thay bằng URL backend thật nhé

    constructor(private http: HttpClient) { }

    // Lấy profile user hiện tại
    getProfile(): Observable<User> {
        return this.http.get<User>(`${this.apiUrl}/profile`);
    }

    updateProfileByAdmin(id: number, dto: UpdateUserProfileDto): Observable<any> {
        return this.http.put(
        `${UserEnviroment.apiUrl}/users/profile?userId=${id}`,  // note query parameter
        dto
        );
    }

    // Cập nhật profile user hiện tại
    updateProfile(profile: UpdateUserProfileDto): Observable<void> {
        return this.http.put<void>(`${this.apiUrl}/profile`, profile);
    }

    // Lấy user theo id (admin dùng)
    getUserById(userId: number): Observable<User> {
        return this.http.get<User>(`${this.apiUrl}/${userId}`);
    }

    // Xóa user theo id (admin dùng)
    deleteUser(userId: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${userId}`);
    }

    // Lấy danh sách users với filter & phân trang
    getUsers(
        page: number,
        pageSize: number,
        roleFilter?: string,
        searchTerm?: string
    ): Observable<{ items: UserWithProfile[]; totalCount: number }> {
        const params: any = {
            page: page.toString(),
            pageSize: pageSize.toString(),
        };
        if (roleFilter) params.roleFilter = roleFilter;
        if (searchTerm) params.searchTerm = searchTerm;

        return this.http.get<{ items: UserWithProfile[]; totalCount: number }>(this.apiUrl, { params });
    }

    changePassword(currentPassword: string, newPassword: string): Observable<void> {
        return this.http.post<void>(`${this.apiUrl}/change-password`, {
            currentPassword,
            newPassword
        });
    }
}
