import { uniqueId } from 'lodash';

import {
  IconAward,
  IconBoxMultiple,
  IconPoint,
  IconBan,
  IconStar,
  IconMoodSmile,
  IconAperture,
  IconMapPin,
  IconSettings,
  IconPlus,
  IconList,
  IconUsers,
  IconScissors,
  IconClipboardList,
  IconGift,
  IconSpa,
  IconPackages,
  IconTrendingDown,
  IconCategory,
} from '@tabler/icons-react';

const Menuitems = [
  {
    navlabel: true,
    subheader: 'Home',
  },

  {
    id: uniqueId(),
    title: 'Dashboard',
    icon: IconAperture,
    href: '/',
    chip: 'New',
    chipColor: 'secondary',
  },
  {
    id: uniqueId(),
    title: 'Sample ',
    icon: IconAperture,
    href: '/sample-page',
  },
  {
    id: uniqueId(),
    title: 'Public Tours',
    icon: IconMapPin,
    href: '/tours',
    external: true,
    chip: 'Public',
    chipColor: 'success',
  },


  {
    navlabel: true,
    subheader: 'Administration',
  },
  {
    id: uniqueId(),
    title: 'Tour Management',
    icon: IconSettings,
    href: '/admin/tours',
    children: [
      {
        id: uniqueId(),
        title: 'All Tours',
        icon: IconList,
        href: '/admin/tours',
      },
      {
        id: uniqueId(),
        title: 'Add New Tour',
        icon: IconPlus,
        href: '/admin/tours/new',
      },
    ],
  },
  {
    id: uniqueId(),
    title: 'Bookings',
    icon: IconClipboardList,
    href: '/admin/bookings',
  },

  {
    navlabel: true,
    subheader: 'Other',
  },
  {
    id: uniqueId(),
    title: 'Menu Level',
    icon: IconBoxMultiple,
    href: '/menulevel/',
    children: [
      {
        id: uniqueId(),
        title: 'Level 1',
        icon: IconPoint,
        href: '/l1',
      },
      {
        id: uniqueId(),
        title: 'Level 1.1',
        icon: IconPoint,
        href: '/l1.1',
        children: [
          {
            id: uniqueId(),
            title: 'Level 2',
            icon: IconPoint,
            href: '/l2',
          },
          {
            id: uniqueId(),
            title: 'Level 2.1',
            icon: IconPoint,
            href: '/l2.1',
            children: [
              {
                id: uniqueId(),
                title: 'Level 3',
                icon: IconPoint,
                href: '/l3',
              },
              {
                id: uniqueId(),
                title: 'Level 3.1',
                icon: IconPoint,
                href: '/l3.1',
              },
            ],
          },
        ],
      },
    ],
  },
  {
    id: uniqueId(),
    title: 'Disabled',
    icon: IconBan,
    href: '',
    disabled: true,
  },
  {
    id: uniqueId(),
    title: 'SubCaption',
    subtitle: 'This is the sutitle',
    icon: IconStar,
    href: '',
  },

  {
    id: uniqueId(),
    title: 'Chip',
    icon: IconAward,
    href: '',
    chip: '9',
    chipColor: 'primary',
  },
  {
    id: uniqueId(),
    title: 'Outlined',
    icon: IconMoodSmile,
    href: '',
    chip: 'outline',
    variant: 'outlined',
    chipColor: 'primary',
  },
  {
    id: uniqueId(),
    title: 'External Link',
    external: true,
    icon: IconStar,
    href: 'https://google.com',
  },
];

export default Menuitems;
