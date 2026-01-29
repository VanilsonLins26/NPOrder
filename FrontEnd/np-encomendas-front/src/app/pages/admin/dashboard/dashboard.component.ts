import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { CardModule } from 'primeng/card';
import { DashboardService } from '../../../core/services/dashboard.service';
import { DashboardStats } from '../../../core/models/dashboard.model';
import { ChartModule } from 'primeng/chart';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, CardModule, ChartModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardAdminComponent implements OnInit {
  private dashboardService = inject(DashboardService);

  stats: DashboardStats | null = null;
  
  barData: any;
  barOptions: any;
  pieData: any;
  pieOptions: any;

  ngOnInit() {
    this.loadData();
    this.initChartOptions();
  }

  loadData() {
    this.dashboardService.getStats().subscribe(data => {
      this.stats = data;
      this.initCharts(data);
    });
  }

  initCharts(data: DashboardStats) {
    this.barData = {
      labels: data.monthlyLabels,
      datasets: [
        {
          label: 'Faturamento (R$)',
          data: data.monthlyRevenue,
          backgroundColor: '#6366f1', 
          borderColor: '#4f46e5',
          borderWidth: 1
        }
      ]
    };

    this.pieData = {
      labels: ['Confirmados', 'Pendentes', 'Cancelados'],
      datasets: [
        {
          data: data.orderStatusCounts,
          backgroundColor: [
            '#22c55e', 
            '#eab308', 
            '#ef4444'  
          ],
          hoverBackgroundColor: ['#16a34a', '#ca8a04', '#dc2626']
        }
      ]
    };
  }

  initChartOptions() {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--text-color');
    const surfaceBorder = documentStyle.getPropertyValue('--surface-border');

    this.barOptions = {
      maintainAspectRatio: false,
      aspectRatio: 0.8,
      plugins: {
        legend: { labels: { color: textColor } }
      },
      scales: {
        x: {
          ticks: { color: textColor, font: { weight: 500 } },
          grid: { color: surfaceBorder, drawBorder: false }
        },
        y: {
          ticks: { color: textColor },
          grid: { color: surfaceBorder, drawBorder: false }
        }
      }
    };

    this.pieOptions = {
        plugins: {
            legend: {
                labels: {
                    usePointStyle: true,
                    color: textColor
                }
            }
        }
    };
  }
}