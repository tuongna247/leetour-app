'use client';

import React from 'react';

const WhyUsSection = () => {
  return (
    <section className="py-10 container mx-auto px-4">
      <h2 className="text-2xl font-bold text-center mb-10">Why Us ?</h2>

      <div className="relative px-8 italic text-gray-700 text-lg max-w-3xl mx-auto text-center">
        <span className="absolute left-0 top-0 text-6xl text-green-600 opacity-50">&quot;</span>
        VINADAY GOREISE is a local tour operator in Ho Chi Minh City, offering daily guaranteed trips to the Cu Chi Tunnels and Mekong Delta. We provide affordable, authentic local experiences with friendly guides, small groups, and the lowest prices. Explore Vietnam the local way - cheap, good, and guaranteed every day.
        <span className="absolute right-0 bottom-[-2rem] text-6xl text-green-600 opacity-50">&quot;</span>
      </div>
    </section>
  );
};

export default WhyUsSection;
