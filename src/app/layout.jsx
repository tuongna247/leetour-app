
import React from "react";
import { CustomizerContextProvider } from "./context/customizerContext";
import { AuthProvider } from "../contexts/AuthContext";
import MyApp from './app';
import NextTopLoader from 'nextjs-toploader';
import "./global.css";


export const metadata = {
  title: 'Modernize Starterkit Demo',
  description: 'Modernize Starterkit kit',
}

export default function RootLayout({ children }) {
  return (
    <html lang="en" suppressHydrationWarning>
      <body>
        <NextTopLoader color="#5D87FF" />
        <AuthProvider>
          <CustomizerContextProvider>
            <MyApp>{children}</MyApp>
          </CustomizerContextProvider>
        </AuthProvider>
      </body>
    </html>
  );
}


