'use client';

import React from 'react';
import Link from 'next/link';

const TopDestinations = () => {
  const destinations = [
    {
      name: 'Ho Chi Minh city & South',
      image: '/images/destinations/hcm.jpg',
      link: '/tours?location=Ho Chi Minh'
    },
    {
      name: 'Hoi An and Central',
      image: '/images/destinations/hoian.jpg',
      link: '/tours?location=Hoi An'
    },
    {
      name: 'Hanoi and North',
      image: '/images/destinations/hanoi.jpg',
      link: '/tours?location=Hanoi'
    }
  ];

  return (
    <section className="py-10 container mx-auto px-4">
      <h2 className="text-2xl font-bold mb-6">Top Destinations</h2>

      <div className="grid grid-cols-2 md:grid-cols-3 lg:grid-cols-5 gap-4">
        {destinations.map((destination, index) => (
          <Link
            key={index}
            href={destination.link}
            className="destination-card relative rounded-lg overflow-hidden h-40"
          >
            <img
              src={destination.image}
              alt={destination.name}
              className="w-full h-full object-cover"
              loading="lazy"
              onError={(e) => {
                e.target.src = '/images/tours/default-tour.jpg';
              }}
            />
            <div className="absolute inset-0 bg-gradient-to-t from-black/70 to-transparent"></div>
            <div className="absolute bottom-3 left-3 text-white font-bold">
              {destination.name}
            </div>
          </Link>
        ))}
      </div>
    </section>
  );
};

export default TopDestinations;
