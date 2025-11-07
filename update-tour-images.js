/**
 * Update Tour Images Script
 *
 * This script updates all tours to use the first gallery image as the primary image
 * instead of placeholder blog images
 */

const mongoose = require('mongoose');
require('dotenv').config();

// MongoDB connection
const MONGODB_URI = process.env.MONGODB_URI || 'mongodb://localhost:27017/leetour';

// Tour schema (simplified)
const tourSchema = new mongoose.Schema({
  title: String,
  images: [{
    url: String,
    alt: String,
    isPrimary: Boolean
  }]
}, { collection: 'tours' });

const Tour = mongoose.model('Tour', tourSchema);

async function updateTourImages() {
  try {
    console.log('Connecting to MongoDB...');
    await mongoose.connect(MONGODB_URI);
    console.log('✓ Connected to MongoDB\n');

    // Find all tours with blog placeholder images
    const tours = await Tour.find({
      'images.url': { $regex: /^\/images\/blog\/blog-im/ }
    });

    console.log(`Found ${tours.length} tours with placeholder images\n`);

    let updatedCount = 0;
    let skippedCount = 0;

    for (const tour of tours) {
      console.log(`Processing: ${tour.title}`);
      console.log(`  Tour ID: ${tour._id}`);

      // Find the first non-blog image in the gallery
      const galleryImage = tour.images.find(img =>
        !img.url.startsWith('/images/blog/blog-im')
      );

      if (galleryImage) {
        // Remove all blog placeholder images
        tour.images = tour.images.filter(img =>
          !img.url.startsWith('/images/blog/blog-im')
        );

        // Set the first gallery image as primary
        tour.images.forEach((img, index) => {
          img.isPrimary = (index === 0);
        });

        await tour.save();
        updatedCount++;
        console.log(`  ✓ Updated - Primary image: ${tour.images[0].url}\n`);
      } else {
        skippedCount++;
        console.log(`  ⚠ Skipped - No gallery images available\n`);
      }
    }

    console.log('\n========================================');
    console.log('Update Summary:');
    console.log(`  Total tours processed: ${tours.length}`);
    console.log(`  ✓ Updated: ${updatedCount}`);
    console.log(`  ⚠ Skipped: ${skippedCount}`);
    console.log('========================================\n');

    // Show sample of updated tours
    if (updatedCount > 0) {
      console.log('Sample of updated tours:');
      const sampleTours = await Tour.find({
        'images.isPrimary': true
      }).limit(5).select('title images');

      sampleTours.forEach((tour, index) => {
        const primaryImage = tour.images.find(img => img.isPrimary);
        console.log(`  ${index + 1}. ${tour.title}`);
        console.log(`     Primary image: ${primaryImage?.url || 'None'}`);
      });
    }

  } catch (error) {
    console.error('Error updating tour images:', error);
  } finally {
    await mongoose.connection.close();
    console.log('\n✓ MongoDB connection closed');
  }
}

// Run the script
updateTourImages();
