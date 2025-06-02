import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, FormControl } from '@angular/forms';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { BreadcrumbComponent } from '@shared/components/breadcrumb/breadcrumb.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ChatService, ChatUser, MessageDto, SendMessageDto } from '../chat/chat.service';
import { AuthService } from '../../core/service/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    NgScrollbarModule,
    BreadcrumbComponent,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class ChatComponent implements OnInit {
  currentUserId: number = 0;
  contacts: ChatUser[] = [];
  selectedContactId: number | null = null;
  messages: MessageDto[] = [];
  newMessageContent: string = '';

  // Các property phục vụ template gốc
  searchTerm: string = '';
  unreadCount: number = 0;
  hideRequiredControl = new FormControl(false);

  // Nếu bạn muốn subscribe hoặc cleanup, giữ reference subscription
  private _subscriptions: Subscription[] = [];

  constructor(
    private chatService: ChatService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    // Lấy userId hiện tại (nếu null thì dừng)
    const id = this.authService.getCurrentUserId();
    if (id === null) {
      console.error('Chưa có user đăng nhập.');
      return;
    }
    this.currentUserId = id;

    // Mock: cho mỗi user một isOnline (bạn có thể gắn thật theo dữ liệu từ API)
    this.loadContacts();
  }

  /**
   * Load danh sách contacts và mock trường isOnline (nếu chưa có).
   */
  private loadContacts(): void {
    const sub = this.chatService.getAllUsers(this.currentUserId).subscribe({
      next: users => {
        // Giả lập thêm thuộc tính isOnline cho mỗi user
        this.contacts = users.map(u => ({
          ...u,
          isOnline: Math.random() < 0.7 // ví dụ: 70% là online
        }));
      },
      error: err => console.error('Lỗi khi load users:', err)
    });
    this._subscriptions.push(sub);
  }

  /**
   * Helper: trả về fullName của contact theo id,
   * Nếu không tìm thấy, trả về 'Người khác'.
   */
  getContactName(id: number): string {
    const user = this.contacts.find(u => u.id === id);
    return user ? user.fullName : 'Người khác';
  }

  /**
   * Getter: trả về tên của contact đang được chọn
   * (hoặc chuỗi rỗng nếu chưa chọn).
   */
  get selectedContactName(): string {
    if (this.selectedContactId === null) {
      return '';
    }
    return this.getContactName(this.selectedContactId);
  }

  /**
   * Helper: lấy avatarUrl của contact đang chat.
   * Tránh viết find(...) trực tiếp trong template.
   */
  getSelectedContactAvatar(): string {
    const user = this.contacts.find(u => u.id === this.selectedContactId!);
    return user ? (user.avatarUrl || 'assets/images/user/default-avatar.png') : 'assets/images/user/default-avatar.png';
  }

  // Khi user click vào 1 contact trong cột trái
  selectContact(contact: ChatUser): void {
    this.selectedContactId = contact.id;
    this.loadMessages();
    // Ví dụ: đặt số tin nhắn chưa đọc = 0 khi chọn vào
    this.unreadCount = 0;
  }

  // Load history chat giữa currentUserId và selectedContactId
  loadMessages(): void {
    if (!this.selectedContactId) return;

    const sub = this.chatService
      .getMessages(this.currentUserId, this.selectedContactId)
      .subscribe({
        next: msgs => {
          this.messages = msgs;
          this.scrollToBottom();
          this.markAllAsRead(msgs);
        },
        error: err => console.error('Lỗi khi load messages:', err)
      });
    this._subscriptions.push(sub);
  }

  // Khi click nút Send
  sendMessage(): void {
    if (!this.newMessageContent.trim() || !this.selectedContactId) return;

    const dto: SendMessageDto = {
      senderId: this.currentUserId,
      receiverId: this.selectedContactId,
      content: this.newMessageContent.trim()
    };

    const sub = this.chatService.sendMessage(dto).subscribe({
      next: sentMsg => {
        this.messages.push(sentMsg);
        this.newMessageContent = '';
        this.scrollToBottom();
      },
      error: err => console.error('Lỗi khi gửi message:', err)
    });
    this._subscriptions.push(sub);
  }

  // Tuỳ chọn: Tự động đánh dấu tin nhắn chưa đọc thành đã đọc
  private markAllAsRead(msgs: MessageDto[]): void {
    msgs.forEach(m => {
      if (!m.isRead && m.receiverId === this.currentUserId) {
        const sub = this.chatService.markAsRead(m.id).subscribe({
          next: () => { /* thành công */ },
          error: e => console.error('Lỗi đánh dấu đã đọc:', e)
        });
        this._subscriptions.push(sub);
      }
    });
  }

  // Cuộn xuống bottom khi có tin nhắn mới
  private scrollToBottom(): void {
    setTimeout(() => {
      const container = document.getElementById('chat-conversation');
      if (container) {
        container.scrollTop = container.scrollHeight;
      }
    }, 100);
  }

  /**
   * Stub method: đính kèm file (nếu có)
   */
  attachFile(): void {
    console.log('attachFile called');
    // TODO: mở input type="file" hoặc làm gì tuỳ bạn
  }

  /**
   * Stub method: mở picker emoji (nếu có)
   */
  openEmojiPicker(): void {
    console.log('openEmojiPicker called');
    // TODO: hiển thị picker emoji
  }

  ngOnDestroy(): void {
    // Hủy tất cả subscription nếu cần
    this._subscriptions.forEach(s => s.unsubscribe());
  }
}
