'use client';

import React, { useState } from 'react';
import { useRouter } from 'next/navigation';

const HeroSection = () => {
  const router = useRouter();
  const [country, setCountry] = useState('');
  const [city, setCity] = useState('');
  const [keyword, setKeyword] = useState('');

  const handleSearch = () => {
    const params = new URLSearchParams();
    if (country) params.append('country', country);
    if (city) params.append('city', city);
    if (keyword) params.append('search', keyword);

    router.push(`/tours?${params.toString()}`);
  };

  return (
    <section className="relative">
      <div className="hero-image w-full h-[500px] bg-cover bg-center relative">
        <div className="absolute inset-0 bg-black opacity-20"></div>
        <div className="absolute top-0 left-0 md:top-1/2 md:translate-y-[-50%] lg:left-1/2 lg:translate-x-[-50%] w-full pt-6 md:pt-0 px-2 md:px-10 flex flex-col items-center justify-center text-white">
          <h1 className="text-4xl font-bold mb-2 text-center">
            From the pulse of Saigon to the poetry of the Mekong - savor Vietnam&apos;s treasures in unparalleled luxury.
          </h1>
          <p className="text-xl mb-8 hidden md:block text-center">
            Join expert-led tours across Southern Vietnam. Daily departures, local guides, unforgettable experiences
          </p>

          {/* Search Form */}
          <div className="w-full max-w-2xl px-4 mt-3">
            <div className="md:bg-white rounded-full md:shadow-lg px-4 py-2">
              <div className="flex flex-wrap md:flex-nowrap">
                <div className="bg-white rounded-lg mb-2 w-full md:w-[57%] p-2 relative md:after:absolute md:after:right-0 md:after:top-1/2 md:after:-translate-y-1/2 md:after:h-10 md:after:w-[1px] md:after:bg-gray-200">
                  <label className="text-xs text-gray-500 block">By City</label>
                  <div className="flex items-center">
                    <div className="w-4/7">
                      <label htmlFor="country-select" className="sr-only">Choose a country</label>
                      <select
                        id="country-select"
                        value={country}
                        onChange={(e) => setCountry(e.target.value)}
                        className="border border-white hover:bg-gray-50 hover:border-gray-300 text-gray-900 text-sm rounded-lg block p-2 w-full truncate"
                      >
                        <option value="">Choose a country</option>
                        <option value="Vietnam">Vietnam</option>
                        <option value="Thailand">Thailand</option>
                        <option value="Laos">Laos</option>
                        <option value="Cambodia">Cambodia</option>
                        <option value="Myanmar">Myanmar</option>
                      </select>
                    </div>
                    <div className="ml-2 w-3/7">
                      <label htmlFor="city-select" className="sr-only">Choose a city</label>
                      <select
                        id="city-select"
                        value={city}
                        onChange={(e) => setCity(e.target.value)}
                        className="border border-white hover:bg-gray-50 hover:border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2 truncate"
                      >
                        <option value="">Choose a city</option>
                        <option value="Ho Chi Minh">Ho Chi Minh</option>
                        <option value="Hanoi">Hanoi</option>
                        <option value="Da Nang">Da Nang</option>
                        <option value="Hoi An">Hoi An</option>
                      </select>
                    </div>
                  </div>
                </div>
                <div className="bg-white rounded-lg mb-2 w-full md:w-[33%] p-2 pl-4">
                  <label className="text-xs text-gray-500 block">By Keywords</label>
                  <div className="flex items-center">
                    <input
                      type="text"
                      id="selectTypeKeyword"
                      placeholder="Where do you want to go?"
                      value={keyword}
                      onChange={(e) => setKeyword(e.target.value)}
                      className="w-full text-gray-800 focus:outline-none text-sm pt-2"
                    />
                  </div>
                </div>
                <button
                  type="button"
                  onClick={handleSearch}
                  className="hidden md:block bg-green-600 hover:bg-green-700 text-white rounded-full px-3 py-2 md:ml-auto self-center"
                >
                  <i className="fas fa-search"></i>
                </button>
                <button
                  type="button"
                  onClick={handleSearch}
                  className="block md:hidden bg-green-600 hover:bg-green-700 text-white rounded-lg w-full px-3 py-2"
                >
                  Search
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default HeroSection;
