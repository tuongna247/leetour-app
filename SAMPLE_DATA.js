// Sample MongoDB Data for SPA Hair Salon
// Use this script to populate your backend database

const { ObjectId } = require('mongodb');

// Sample Services - Insert these first
const sampleServices = [
  {
    _id: new ObjectId("65a1b2c3d4e5f6789abcdef1"),
    name: "Classic Hair Cut",
    description: "Professional hair cutting with styling",
    price: 45,
    duration: 60, // minutes
    category: "Hair Cut",
    isActive: true,
    isComboEligible: true,
    createdAt: new Date("2024-01-01T09:00:00.000Z"),
    updatedAt: new Date("2024-01-01T09:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1b2c3d4e5f6789abcdef2"),
    name: "Hair Wash & Blow Dry",
    description: "Deep cleansing wash with professional blow dry",
    price: 30,
    duration: 45,
    category: "Hair Treatment",
    isActive: true,
    isComboEligible: true,
    createdAt: new Date("2024-01-01T09:00:00.000Z"),
    updatedAt: new Date("2024-01-01T09:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1b2c3d4e5f6789abcdef3"),
    name: "Hair Color Treatment",
    description: "Full hair coloring service with premium products",
    price: 120,
    duration: 180,
    category: "Hair Color",
    isActive: true,
    isComboEligible: true,
    createdAt: new Date("2024-01-01T09:00:00.000Z"),
    updatedAt: new Date("2024-01-01T09:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1b2c3d4e5f6789abcdef4"),
    name: "Deep Hair Treatment",
    description: "Intensive hair repair and conditioning treatment",
    price: 80,
    duration: 90,
    category: "Hair Treatment",
    isActive: true,
    isComboEligible: true,
    createdAt: new Date("2024-01-01T09:00:00.000Z"),
    updatedAt: new Date("2024-01-01T09:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1b2c3d4e5f6789abcdef5"),
    name: "Relaxing Facial",
    description: "60-minute facial treatment with organic products",
    price: 95,
    duration: 60,
    category: "Facial",
    isActive: true,
    isComboEligible: true,
    createdAt: new Date("2024-01-01T09:00:00.000Z"),
    updatedAt: new Date("2024-01-01T09:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1b2c3d4e5f6789abcdef6"),
    name: "Therapeutic Massage",
    description: "Full body massage for relaxation and stress relief",
    price: 110,
    duration: 75,
    category: "Massage",
    isActive: true,
    isComboEligible: true,
    createdAt: new Date("2024-01-01T09:00:00.000Z"),
    updatedAt: new Date("2024-01-01T09:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1b2c3d4e5f6789abcdef7"),
    name: "Manicure",
    description: "Professional nail care and polish application",
    price: 35,
    duration: 45,
    category: "Other",
    isActive: true,
    isComboEligible: false, // Not eligible for combo
    createdAt: new Date("2024-01-01T09:00:00.000Z"),
    updatedAt: new Date("2024-01-01T09:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1b2c3d4e5f6789abcdef8"),
    name: "Hair Styling for Events",
    description: "Special occasion hair styling and makeup",
    price: 85,
    duration: 120,
    category: "Hair Styling",
    isActive: true,
    isComboEligible: true,
    createdAt: new Date("2024-01-01T09:00:00.000Z"),
    updatedAt: new Date("2024-01-01T09:00:00.000Z")
  }
];

// Sample Customers - Insert these after services
const sampleCustomers = [
  {
    _id: new ObjectId("65a1c2d3e4f5678901234567"),
    fullName: "Emma Johnson",
    phoneNumber: "+1-555-0123",
    facebook: "emma.johnson.spa",
    dateOfBirth: new Date("1988-03-15T00:00:00.000Z"),
    totalServices: 15,
    currentCombo: {
      servicesPurchased: 12,
      servicesUsed: 10,
      freeServicesEarned: 2, // Math.floor(12/10) * 2 = 2
      freeServicesUsed: 1,
      comboStartDate: new Date("2024-01-15T10:30:00.000Z"),
      comboExpiryDate: new Date("2025-01-15T00:00:00.000Z")
    },
    isActive: true,
    notes: "Prefers morning appointments. Allergic to certain hair dyes - check product list before coloring.",
    lastVisit: new Date("2024-12-01T14:30:00.000Z"),
    createdAt: new Date("2024-01-15T10:30:00.000Z"),
    updatedAt: new Date("2024-12-01T14:30:00.000Z")
  },
  {
    _id: new ObjectId("65a1c2d3e4f5678901234568"),
    fullName: "Sofia Rodriguez",
    phoneNumber: "+1-555-0456",
    facebook: "sofia.rodriguez.beauty",
    dateOfBirth: new Date("1992-07-22T00:00:00.000Z"),
    totalServices: 24,
    currentCombo: {
      servicesPurchased: 22,
      servicesUsed: 18,
      freeServicesEarned: 4, // Math.floor(22/10) * 2 = 4
      freeServicesUsed: 2,
      comboStartDate: new Date("2023-11-01T09:00:00.000Z"),
      comboExpiryDate: new Date("2024-11-01T00:00:00.000Z") // This will show as expiring soon!
    },
    isActive: true,
    notes: "Regular customer. Loves trying new hair treatments. Prefers afternoon slots.",
    lastVisit: new Date("2024-11-25T16:00:00.000Z"),
    createdAt: new Date("2023-11-01T09:00:00.000Z"),
    updatedAt: new Date("2024-11-25T16:00:00.000Z")
  }
];

// Sample Customer Service Records - Insert these last
const sampleCustomerServices = [
  // Emma Johnson's service history
  {
    _id: new ObjectId("65a1d2e3f4567890abcdef01"),
    customerId: new ObjectId("65a1c2d3e4f5678901234567"),
    serviceId: new ObjectId("65a1b2c3d4e5f6789abcdef1"),
    serviceName: "Classic Hair Cut",
    servicePrice: 45,
    isPaid: true,
    isFreeService: false,
    serviceDate: new Date("2024-01-15T10:30:00.000Z"),
    staffName: "Alice Chen",
    status: "Completed",
    notes: "First visit - customer was very happy with the service",
    rating: 5,
    createdAt: new Date("2024-01-15T10:30:00.000Z"),
    updatedAt: new Date("2024-01-15T10:30:00.000Z")
  },
  {
    _id: new ObjectId("65a1d2e3f4567890abcdef02"),
    customerId: new ObjectId("65a1c2d3e4f5678901234567"),
    serviceId: new ObjectId("65a1b2c3d4e5f6789abcdef2"),
    serviceName: "Hair Wash & Blow Dry",
    servicePrice: 30,
    isPaid: true,
    isFreeService: false,
    serviceDate: new Date("2024-02-10T11:00:00.000Z"),
    staffName: "Maria Santos",
    status: "Completed",
    notes: "Regular maintenance appointment",
    rating: 5,
    createdAt: new Date("2024-02-10T11:00:00.000Z"),
    updatedAt: new Date("2024-02-10T11:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1d2e3f4567890abcdef03"),
    customerId: new ObjectId("65a1c2d3e4f5678901234567"),
    serviceId: new ObjectId("65a1b2c3d4e5f6789abcdef5"),
    serviceName: "Relaxing Facial",
    servicePrice: 95,
    isPaid: false,
    isFreeService: true, // This is a free service Emma earned
    serviceDate: new Date("2024-11-01T14:00:00.000Z"),
    staffName: "Lisa Wang",
    status: "Completed",
    notes: "Used first earned free service - customer loved the facial treatment",
    rating: 5,
    createdAt: new Date("2024-11-01T14:00:00.000Z"),
    updatedAt: new Date("2024-11-01T14:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1d2e3f4567890abcdef04"),
    customerId: new ObjectId("65a1c2d3e4f5678901234567"),
    serviceId: new ObjectId("65a1b2c3d4e5f6789abcdef3"),
    serviceName: "Hair Color Treatment",
    servicePrice: 120,
    isPaid: true,
    isFreeService: false,
    serviceDate: new Date("2024-12-01T14:30:00.000Z"),
    staffName: "Jennifer Kim",
    status: "Completed",
    notes: "Beautiful blonde highlights - customer was thrilled with the results",
    rating: 5,
    createdAt: new Date("2024-12-01T14:30:00.000Z"),
    updatedAt: new Date("2024-12-01T14:30:00.000Z")
  },

  // Sofia Rodriguez's service history
  {
    _id: new ObjectId("65a1d2e3f4567890abcdef05"),
    customerId: new ObjectId("65a1c2d3e4f5678901234568"),
    serviceId: new ObjectId("65a1b2c3d4e5f6789abcdef4"),
    serviceName: "Deep Hair Treatment",
    servicePrice: 80,
    isPaid: true,
    isFreeService: false,
    serviceDate: new Date("2023-11-01T15:00:00.000Z"),
    staffName: "Sarah Johnson",
    status: "Completed",
    notes: "First visit - hair was damaged from previous salon",
    rating: 4,
    createdAt: new Date("2023-11-01T15:00:00.000Z"),
    updatedAt: new Date("2023-11-01T15:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1d2e3f4567890abcdef06"),
    customerId: new ObjectId("65a1c2d3e4f5678901234568"),
    serviceId: new ObjectId("65a1b2c3d4e5f6789abcdef6"),
    serviceName: "Therapeutic Massage",
    servicePrice: 110,
    isPaid: false,
    isFreeService: true, // Sofia's first free service
    serviceDate: new Date("2024-09-15T16:30:00.000Z"),
    staffName: "Monica Lee",
    status: "Completed",
    notes: "Used first free service - very relaxing session",
    rating: 5,
    createdAt: new Date("2024-09-15T16:30:00.000Z"),
    updatedAt: new Date("2024-09-15T16:30:00.000Z")
  },
  {
    _id: new ObjectId("65a1d2e3f4567890abcdef07"),
    customerId: new ObjectId("65a1c2d3e4f5678901234568"),
    serviceId: new ObjectId("65a1b2c3d4e5f6789abcdef8"),
    serviceName: "Hair Styling for Events",
    servicePrice: 85,
    isPaid: false,
    isFreeService: true, // Sofia's second free service
    serviceDate: new Date("2024-10-20T13:00:00.000Z"),
    staffName: "Alice Chen",
    status: "Completed",
    notes: "Special styling for wedding event - looked amazing",
    rating: 5,
    createdAt: new Date("2024-10-20T13:00:00.000Z"),
    updatedAt: new Date("2024-10-20T13:00:00.000Z")
  },
  {
    _id: new ObjectId("65a1d2e3f4567890abcdef08"),
    customerId: new ObjectId("65a1c2d3e4f5678901234568"),
    serviceId: new ObjectId("65a1b2c3d4e5f6789abcdef1"),
    serviceName: "Classic Hair Cut",
    servicePrice: 45,
    isPaid: true,
    isFreeService: false,
    serviceDate: new Date("2024-11-25T16:00:00.000Z"),
    staffName: "Jennifer Kim",
    status: "Completed",
    notes: "Regular trim and style",
    rating: 5,
    createdAt: new Date("2024-11-25T16:00:00.000Z"),
    updatedAt: new Date("2024-11-25T16:00:00.000Z")
  }
];

// MongoDB Insert Scripts
console.log("=== MongoDB Insert Commands ===\n");

console.log("// 1. Insert Services");
console.log("db.services.insertMany(" + JSON.stringify(sampleServices, null, 2) + ");\n");

console.log("// 2. Insert Customers");
console.log("db.customers.insertMany(" + JSON.stringify(sampleCustomers, null, 2) + ");\n");

console.log("// 3. Insert Customer Service Records");
console.log("db.customerservices.insertMany(" + JSON.stringify(sampleCustomerServices, null, 2) + ");\n");

// Export for use in Node.js backend
module.exports = {
  sampleServices,
  sampleCustomers,
  sampleCustomerServices
};

// Usage Instructions:
console.log(`
=== USAGE INSTRUCTIONS ===

1. Connect to your MongoDB database
2. Run the insert commands above in MongoDB Compass or mongo shell
3. Or use this in your Node.js backend:

const { sampleServices, sampleCustomers, sampleCustomerServices } = require('./SAMPLE_DATA.js');

// Insert data
await Service.insertMany(sampleServices);
await Customer.insertMany(sampleCustomers);
await CustomerService.insertMany(sampleCustomerServices);

=== SAMPLE DATA SUMMARY ===

Services: 8 total
- 7 combo-eligible services (Hair Cut, Treatments, Facial, Massage, Styling)
- 1 non-combo service (Manicure)

Customers: 2 total
- Emma Johnson: 12 purchased services, 1 free service used, 1 free service available
- Sofia Rodriguez: 22 purchased services, 2 free services used, 2 free services available
  * Sofia's combo expires on 2024-11-01 (will show as expiring!)

Service Records: 8 total
- 6 paid services
- 2 free services used
- Various staff members and ratings

This data will demonstrate:
✓ Combo progression (buy 10 get 2 free)
✓ Free service usage
✓ Expiring combo alerts
✓ Service history tracking
✓ Customer management features
`);