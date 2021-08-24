import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: '1. MAINTAIN',
    url: '/repair',
    icon: 'icon-list',
    children: [
      {
        name: '1.1 Feedback Repair',
        url: '/repair/repair'
      }
    ]
  },
  {
    name: '3. KANBAN',
    url: '/officekanban',
    icon: 'icon-list',
    children: [
      {
        name: '3.1 IT Repair Kanban',
        url: '/officekanban/officekanban'
      }
    ]
  },
];
