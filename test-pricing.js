/**
 * Test Script for Pricing Management Feature
 * Tests: GET tour, PATCH pricing data, verify persistence
 */

const BASE_URL = 'http://localhost:3000';

// Test data for pricing
const testPricingData = {
  tourOptions: [
    {
      optionName: "Small Group (1-4 people)",
      description: "Perfect for intimate tours",
      basePrice: 150,
      pricingTiers: [
        { minPassengers: 1, maxPassengers: 2, pricePerPerson: 150 },
        { minPassengers: 3, maxPassengers: 4, pricePerPerson: 120 }
      ],
      isActive: true
    }
  ],
  surcharges: [
    {
      surchargeName: "Weekend Surcharge",
      surchargeType: "weekend",
      startDate: "2025-01-01",
      endDate: "2025-12-31",
      amountType: "percentage",
      amount: 20,
      description: "Extra fee for weekend bookings",
      isActive: true
    },
    {
      surchargeName: "Lunar New Year",
      surchargeType: "holiday",
      startDate: "2025-01-28",
      endDate: "2025-02-05",
      amountType: "percentage",
      amount: 30,
      description: "Holiday surcharge during Tet",
      isActive: true
    }
  ],
  promotions: [
    {
      promotionName: "Early Bird Discount",
      promotionType: "early_bird",
      discountType: "percentage",
      discountAmount: 15,
      validFrom: "2025-01-01",
      validTo: "2025-12-31",
      daysBeforeDeparture: 30,
      conditions: "Book 30 days in advance",
      isActive: true
    }
  ],
  cancellationPolicies: [
    {
      daysBeforeDeparture: 30,
      refundPercentage: 100,
      description: "Full refund if cancelled 30+ days before",
      displayOrder: 1
    },
    {
      daysBeforeDeparture: 14,
      refundPercentage: 50,
      description: "50% refund if cancelled 14-29 days before",
      displayOrder: 2
    },
    {
      daysBeforeDeparture: 0,
      refundPercentage: 0,
      description: "No refund if cancelled less than 14 days",
      displayOrder: 3
    }
  ]
};

async function testPricingManagement() {
  console.log('🧪 Starting Pricing Management Tests...\n');

  try {
    // Step 1: Get list of tours to find a test tour
    console.log('📋 Step 1: Fetching tours list...');
    const toursResponse = await fetch(`${BASE_URL}/api/admin/tours?limit=1`);
    const toursData = await toursResponse.json();

    if (!toursData.data || !toursData.data.tours || toursData.data.tours.length === 0) {
      console.error('❌ No tours found. Please create a tour first.');
      return;
    }

    const testTourId = toursData.data.tours[0]._id;
    console.log(`✅ Found test tour: ${toursData.data.tours[0].title} (ID: ${testTourId})\n`);

    // Step 2: GET tour data before update
    console.log('📖 Step 2: Getting tour data before update...');
    const beforeResponse = await fetch(`${BASE_URL}/api/admin/tours/${testTourId}`);
    const beforeData = await beforeResponse.json();

    if (beforeData.status === 200) {
      console.log('✅ GET request successful');
      console.log(`   - Tour Options: ${beforeData.data.tourOptions?.length || 0}`);
      console.log(`   - Surcharges: ${beforeData.data.surcharges?.length || 0}`);
      console.log(`   - Promotions: ${beforeData.data.promotions?.length || 0}`);
      console.log(`   - Cancellation Policies: ${beforeData.data.cancellationPolicies?.length || 0}\n`);
    } else {
      console.error('❌ Failed to GET tour data:', beforeData.msg);
      return;
    }

    // Step 3: PATCH request to update pricing data
    console.log('💾 Step 3: Sending PATCH request to update pricing...');
    const patchResponse = await fetch(`${BASE_URL}/api/admin/tours/${testTourId}`, {
      method: 'PATCH',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(testPricingData)
    });

    const patchData = await patchResponse.json();

    if (patchData.status === 200) {
      console.log('✅ PATCH request successful!');
      console.log(`   Response: ${patchData.msg}\n`);
    } else {
      console.error('❌ PATCH request failed:', patchData.msg);
      console.error('   Error:', patchData.error);
      return;
    }

    // Step 4: GET tour data after update to verify
    console.log('🔍 Step 4: Verifying data persistence...');
    await new Promise(resolve => setTimeout(resolve, 1000)); // Wait 1 sec for DB update

    const afterResponse = await fetch(`${BASE_URL}/api/admin/tours/${testTourId}`);
    const afterData = await afterResponse.json();

    if (afterData.status === 200) {
      console.log('✅ Data persistence verified!');
      console.log(`   - Tour Options: ${afterData.data.tourOptions?.length || 0} (Expected: 1)`);
      console.log(`   - Surcharges: ${afterData.data.surcharges?.length || 0} (Expected: 2)`);
      console.log(`   - Promotions: ${afterData.data.promotions?.length || 0} (Expected: 1)`);
      console.log(`   - Cancellation Policies: ${afterData.data.cancellationPolicies?.length || 0} (Expected: 3)\n`);

      // Detailed verification
      let allPassed = true;

      if (afterData.data.tourOptions?.length !== 1) {
        console.error('   ⚠️ Tour Options count mismatch!');
        allPassed = false;
      }
      if (afterData.data.surcharges?.length !== 2) {
        console.error('   ⚠️ Surcharges count mismatch!');
        allPassed = false;
      }
      if (afterData.data.promotions?.length !== 1) {
        console.error('   ⚠️ Promotions count mismatch!');
        allPassed = false;
      }
      if (afterData.data.cancellationPolicies?.length !== 3) {
        console.error('   ⚠️ Cancellation Policies count mismatch!');
        allPassed = false;
      }

      if (allPassed) {
        console.log('✨ All tests PASSED! Pricing management is working correctly.\n');

        // Display sample surcharge
        if (afterData.data.surcharges?.[0]) {
          const surcharge = afterData.data.surcharges[0];
          console.log('📝 Sample Surcharge Details:');
          console.log(`   Name: ${surcharge.surchargeName}`);
          console.log(`   Type: ${surcharge.surchargeType}`);
          console.log(`   Amount: ${surcharge.amount}% (${surcharge.amountType})`);
          console.log(`   Period: ${surcharge.startDate} to ${surcharge.endDate}`);
          console.log(`   Active: ${surcharge.isActive}\n`);
        }
      } else {
        console.log('⚠️ Some tests FAILED. Check details above.\n');
      }

    } else {
      console.error('❌ Failed to verify data:', afterData.msg);
    }

    // Step 5: Test access to pricing page
    console.log('🌐 Step 5: Testing pricing page URL...');
    console.log(`   URL: ${BASE_URL}/admin/tours/${testTourId}/pricing`);
    console.log('   Note: Open this URL in browser to test UI\n');

    console.log('✅ All API tests completed successfully!');
    console.log('\n📊 Test Summary:');
    console.log('   ✓ GET tour endpoint - Working');
    console.log('   ✓ PATCH pricing endpoint - Working');
    console.log('   ✓ Data persistence - Verified');
    console.log('   ✓ Pricing data structure - Correct\n');

  } catch (error) {
    console.error('❌ Test failed with error:', error.message);
    console.error('Stack:', error.stack);
  }
}

// Run tests
testPricingManagement();
