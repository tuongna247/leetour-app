'use client';

import React, { useState } from 'react';
import { useRouter } from 'next/navigation';

const TourCardTailwind = ({ tour }) => {
  const router = useRouter();
  const [isFavorite, setIsFavorite] = useState(false);

  const primaryImage = tour.images?.find(img => img.isPrimary) || tour.images?.[0];
  const tourUrl = `/tours/${tour.seo?.slug || tour._id}`;

  const getBadge = () => {
    if (tour.isFeatured) return { text: 'Featured', color: 'bg-pink-600' };
    if (tour.discountPercentage > 0) return { text: 'Special Offer', color: 'bg-pink-600' };
    return null;
  };

  const badge = getBadge();

  // Calculate star rating display
  const fullStars = Math.floor(tour.rating?.average || 0);
  const hasHalfStar = (tour.rating?.average || 0) % 1 >= 0.5;

  return (
    <div className="bg-white rounded-lg overflow-hidden shadow-md">
      <a href={tourUrl}>
        <div className="relative">
          <img
            src={primaryImage?.url || '/images/tours/default-tour.jpg'}
            alt={tour.title}
            className="w-full h-48 object-cover"
            loading="lazy"
            onError={(e) => {
              e.target.src = '/images/tours/default-tour.jpg';
            }}
          />
          {badge && (
            <div className={`absolute top-2 left-2 ${badge.color} text-white text-xs px-2 py-1 rounded`}>
              {badge.text}
            </div>
          )}
          <button
            onClick={(e) => {
              e.preventDefault();
              setIsFavorite(!isFavorite);
            }}
            className="absolute top-2 right-2 bg-white rounded-full p-1.5 shadow-md"
          >
            <i className={`${isFavorite ? 'fas' : 'far'} fa-heart ${isFavorite ? 'text-red-500' : 'text-gray-500'}`}></i>
          </button>
        </div>
        <div className="p-4">
          <div className="flex items-center text-xs text-gray-600 mb-2">
            <span className="mr-2">{tour.rating?.average?.toFixed(1) || '0.0'} ({tour.rating?.count || 0}+)</span>
            <span className="flex items-center">
              {[...Array(fullStars)].map((_, i) => (
                <i key={i} className="fas fa-star text-yellow-400 mr-1"></i>
              ))}
              {hasHalfStar && <i className="fas fa-star-half-alt text-yellow-400"></i>}
            </span>
          </div>
          <h3 className="font-bold text-sm mb-2">{tour.title}</h3>
          <div className="flex justify-between items-end mt-4">
            <div>
              <span className="text-xs text-gray-500">from</span>
              <div className="font-bold">${tour.price}</div>
              <span className="text-xs text-gray-500">per person</span>
            </div>
          </div>
        </div>
      </a>
    </div>
  );
};

export default TourCardTailwind;
