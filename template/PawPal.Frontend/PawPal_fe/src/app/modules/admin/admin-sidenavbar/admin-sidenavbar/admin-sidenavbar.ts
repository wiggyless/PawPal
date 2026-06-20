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
    Reported_Posts: '/admin/reported-users',
    Settings: '/admin/settings',
  };
  keepOrder = (a: any, b: any) => 0;
  onSelect(item: string) {
    this.selItem = item;
    this.cd.detectChanges();
  }
}
