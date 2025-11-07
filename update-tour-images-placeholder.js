/**
 * Update Tour Images to Professional Placeholder
 *
 * This script replaces blog placeholder images with the default tour image
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

// Default tour image path
const DEFAULT_TOUR_IMAGE = '/images/tours/default-tour.jpg';

async function updateTourImages() {
  try {
    console.log('Connecting to MongoDB...');
    await mongoose.connect(MONGODB_URI);
    console.log('✓ Connected to MongoDB\n');

    // Find all tours with blog placeholder images
    const tours = await Tour.find({
      'images.url': { $regex: /^\/images\/blog\/blog-im/ }
    });

    console.log(`Found ${tours.length} tours with blog placeholder images\n`);

    let updatedCount = 0;

    for (const tour of tours) {
      console.log(`Processing: ${tour.title}`);
      console.log(`  Tour ID: ${tour._id}`);

      // Check if tour has real uploaded images (from Cloudinary)
      const hasRealImages = tour.images.some(img =>
        img.url.includes('cloudinary.com') ||
        (!img.url.startsWith('/images/blog/') && img.url !== DEFAULT_TOUR_IMAGE)
      );

      if (hasRealImages) {
        // Remove blog placeholder images, keep real images
        tour.images = tour.images.filter(img =>
          !img.url.startsWith('/images/blog/blog-im')
        );

        // Set first real image as primary
        tour.images.forEach((img, index) => {
          img.isPrimary = (index === 0);
        });

        console.log(`  ✓ Using uploaded image: ${tour.images[0].url}\n`);
      } else {
        // Replace blog images with default tour image
        tour.images = [{
          url: DEFAULT_TOUR_IMAGE,
          alt: tour.title,
          isPrimary: true
        }];

        console.log(`  ✓ Set default tour image\n`);
      }

      await tour.save();
      updatedCount++;
    }

    console.log('\n========================================');
    console.log('Update Summary:');
    console.log(`  Total tours processed: ${tours.length}`);
    console.log(`  ✓ Updated: ${updatedCount}`);
    console.log('========================================\n');

    // Show sample of updated tours
    console.log('Sample of updated tours:');
    const sampleTours = await Tour.find({}).limit(10).select('title images');

    sampleTours.forEach((tour, index) => {
      const primaryImage = tour.images.find(img => img.isPrimary);
      console.log(`  ${index + 1}. ${tour.title}`);
      console.log(`     Primary: ${primaryImage?.url || 'None'}`);
      console.log(`     Total images: ${tour.images.length}`);
    });

  } catch (error) {
    console.error('Error updating tour images:', error);
  } finally {
    await mongoose.connection.close();
    console.log('\n✓ MongoDB connection closed');
  }
}

// Run the script
updateTourImages();
