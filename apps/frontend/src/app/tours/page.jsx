'use client';

import React, { useState, useEffect } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import HeroSection from '@/app/components/HeroSection';
import WhyUsSection from '@/app/components/WhyUsSection';
import TopDestinations from '@/app/components/TopDestinations';
import TourCardTailwind from '@/app/components/TourCardTailwind';

const ToursPageNew = () => {
  const [tours, setTours] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const searchParams = useSearchParams();

  useEffect(() => {
    fetchTours();
  }, [searchParams]);

  const fetchTours = async () => {
    setLoading(true);
    try {
      const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';
      const queryString = searchParams.toString();
      const response = await fetch(`${apiUrl}/api/tours?${queryString}`);
      const data = await response.json();

      if (data.status === 200) {
        setTours(data.data.tours);
      } else {
        setError(data.msg);
      }
    } catch (err) {
      setError('Failed to fetch tours');
      console.error('Error fetching tours:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      {/* Hero Section with Search */}
      <HeroSection />

      {/* Why Book with Vinaday Goreise */}
      <WhyUsSection />

      {/* Top Tours */}
      <section className="py-10 container mx-auto px-4">
        <h2 className="text-2xl font-bold mb-6">Top Tours</h2>

        {loading ? (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            {[...Array(8)].map((_, i) => (
              <div key={i} className="bg-white rounded-lg overflow-hidden shadow-md animate-pulse">
                <div className="h-48 bg-gray-300"></div>
                <div className="p-4">
                  <div className="h-4 bg-gray-300 rounded mb-2"></div>
                  <div className="h-4 bg-gray-300 rounded w-3/4"></div>
                </div>
              </div>
            ))}
          </div>
        ) : error ? (
          <div className="text-center py-10">
            <p className="text-red-600">{error}</p>
          </div>
        ) : tours.length === 0 ? (
          <div className="text-center py-10">
            <p className="text-gray-600">No tours found</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            {tours.map((tour) => (
              <TourCardTailwind key={tour._id} tour={tour} />
            ))}
          </div>
        )}
      </section>

      {/* Top Destinations */}
      <TopDestinations />

      {/* Keep things flexible */}
      <section className="py-10 bg-green-50">
        <div className="container mx-auto px-4">
          <h2 className="text-2xl font-bold text-center mb-4">
            Enjoy Flexibility in Planning
          </h2>
          <p className="text-center text-gray-600 max-w-2xl mx-auto">
            Save your spot on top tours today and keep your plans open with Pay Later flexibility.
          </p>
        </div>
      </section>

      {/* Free Cancellation */}
      <section className="py-10 bg-green-50">
        <div className="container mx-auto px-4 text-center">
          <h2 className="text-2xl font-bold mb-4">Free cancellation</h2>
          <p className="text-gray-600 max-w-2xl mx-auto">
            You'll receive a full refund if you cancel at least 24 hours in advance of most experiences.
          </p>
        </div>
      </section>
    </div>
  );
};

export default ToursPageNew;
