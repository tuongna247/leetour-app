import { IconPoint, IconAperture, IconBoxMultiple, IconMapPin, IconSettings, IconList, IconPlus, IconClipboardList } from "@tabler/icons-react";
import { uniqueId } from "lodash";

const Menuitems = [
  {
    id: uniqueId(),
    title: "Tours",
    icon: IconMapPin,
    href: "/tours",
  },
  {
    id: uniqueId(),
    title: "Bookings",
    icon: IconClipboardList,
    href: "/admin/bookings",
  },
  {
    id: uniqueId(),
    title: "Tour Management",
    icon: IconSettings,
    href: "/admin/tours",
    children: [
      {
        id: uniqueId(),
        title: "All Tours",
        icon: IconList,
        href: "/admin/tours",
      },
      {
        id: uniqueId(),
        title: "Add New Tour",
        icon: IconPlus,
        href: "/admin/tours/new",
      },
    ],
  },
];
export default Menuitems;
