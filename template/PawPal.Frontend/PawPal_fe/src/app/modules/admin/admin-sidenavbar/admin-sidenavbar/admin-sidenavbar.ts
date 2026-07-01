import { ChangeDetectorRef, Component, inject, Input } from '@angular/core';

@Component({
  selector: 'app-admin-sidenavbar',
  standalone: false,
  templateUrl: './admin-sidenavbar.html',
  styleUrl: './admin-sidenavbar.scss',
})
export class AdminSidenavbar {
  @Input() selItem: string | null = null;
  cd = inject(ChangeDetectorRef);
  matListItems = {
    My_Profile: '/admin/my-profile',
    User_Profiles: '/admin/user-profiles',
    Reported_Users: '/admin/reported-users',
    Reported_Posts: '/admin/reported-posts',
    Reported_Comments: '/admin/reported-comments',
    Reported_Problems: '/admin/reported-problems',
    Disabled_Users: '/admin/disabled-users',
    Settings: '/admin/settings',
  };
  keepOrder = (a: any, b: any) => 0;
  onSelect(item: string) {
    this.selItem = item;
    this.cd.detectChanges();
  }
}
