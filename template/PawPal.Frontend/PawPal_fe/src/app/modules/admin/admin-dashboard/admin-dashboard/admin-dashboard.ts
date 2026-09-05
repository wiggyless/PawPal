import { Component, inject, OnInit, signal } from '@angular/core';
import { DashboardService } from '../../../../api-services/dashboard/dashboard.service';
import { GetDashboardSummaryDto } from '../../../../api-services/dashboard/dashboard.model';

interface StatTile {
  label: string;
  value: number;
  icon: string;
  route?: string;
}

@Component({
  selector: 'app-admin-dashboard',
  standalone: false,
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.scss',
})
export class AdminDashboard implements OnInit {
  dashboardService = inject(DashboardService);
  isLoaded = signal(false);
  hasError = signal(false);
  activityTiles = signal<StatTile[]>([]);
  moderationTiles = signal<StatTile[]>([]);

  ngOnInit(): void {
    this.loadSummary();
  }

  loadSummary(): void {
    this.isLoaded.set(false);
    this.hasError.set(false);
    this.dashboardService.getSummary().subscribe({
      next: (summary: GetDashboardSummaryDto) => {
        this.activityTiles.set([
          { label: 'Active Listings', value: summary.activeListings, icon: 'pets' },
          {
            label: 'Incoming Adoption Requests',
            value: summary.pendingAdoptionRequests,
            icon: 'assignment_turned_in',
          },
        ]);
        this.moderationTiles.set([
          {
            label: 'Reported Posts',
            value: summary.reportedPosts,
            icon: 'report',
            route: '/admin/reported-posts',
          },
          {
            label: 'Reported Users',
            value: summary.reportedUsers,
            icon: 'person_off',
            route: '/admin/reported-users',
          },
          {
            label: 'Reported Comments',
            value: summary.reportedComments,
            icon: 'forum',
            route: '/admin/reported-comments',
          },
          {
            label: 'Reported Problems',
            value: summary.reportedProblems,
            icon: 'report_problem',
            route: '/admin/reported-problems',
          },
        ]);
        this.isLoaded.set(true);
      },
      error: () => {
        this.hasError.set(true);
        this.isLoaded.set(true);
      },
    });
  }
}
