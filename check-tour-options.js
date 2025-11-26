// Quick script to check if tour has tourOptions
// Run with: node check-tour-options.js

const mongoose = require('mongoose');

const MONGODB_URI = 'mongodb://localhost:27017/leetour';

async function checkTour() {
  try {
    await mongoose.connect(MONGODB_URI);
    console.log('✓ Connected to MongoDB\n');

    const Tour = mongoose.model('Tour', new mongoose.Schema({}, { strict: false }));

    // Find tour by slug
    const tour = await Tour.findOne({ 'seo.slug': 'indian-golden-triangle-tour' });

    if (!tour) {
      console.log('❌ Tour not found with slug: indian-golden-triangle-tour\n');

      // List available tours
      const tours = await Tour.find({}, { title: 1, 'seo.slug': 1 }).limit(5);
      console.log('Available tours:');
      tours.forEach(t => {
        console.log(`  - ${t.title} (slug: ${t['seo']?.slug || 'no slug'})`);
      });
      return;
    }

    console.log(`✓ Found tour: ${tour.title}\n`);

    // Check tourOptions
    if (!tour.tourOptions || tour.tourOptions.length === 0) {
      console.log('❌ Tour has NO tourOptions!\n');
      console.log('This is why you get "No tour options available for this tour"\n');
      console.log('Solution: Add tourOptions to this tour in the database.\n');
      console.log('Example tourOption:');
      console.log(`
{
  optionName: "Standard Package",
  description: "Full tour with accommodation",
  basePrice: 50000,
  departureTimes: "08:00 AM;02:00 PM",
  isActive: true
}
      `);
    } else {
      console.log(`✓ Tour has ${tour.tourOptions.length} option(s):\n`);
      tour.tourOptions.forEach((opt, i) => {
        console.log(`Option ${i + 1}:`);
        console.log(`  - Name: ${opt.optionName}`);
        console.log(`  - Base Price: ${opt.basePrice}`);
        console.log(`  - Departure Times: ${opt.departureTimes || 'Not set'}`);
        console.log(`  - Active: ${opt.isActive}`);
        console.log('');
      });
    }

  } catch (error) {
    console.error('Error:', error.message);
  } finally {
    await mongoose.disconnect();
  }
}

checkTour();
