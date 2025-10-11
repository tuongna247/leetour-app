"use client";
import React, { useContext } from "react";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import RTL from "@/app/(DashboardLayout)/layout/shared/customizer/RTL";
import { ThemeSettings } from "@/utils/theme/Theme";
import { CustomizerContext } from '@/app/context/customizerContext';
import { AppRouterCacheProvider } from '@mui/material-nextjs/v14-appRouter';
import { LanguageCurrencyProvider } from '@/contexts/LanguageCurrencyContext';
import "@/utils/i18n";


const MyApp = ({ children }) => {
    const theme = ThemeSettings();
    const { activeDir } = useContext(CustomizerContext);
    return (
        <>
            <AppRouterCacheProvider options={{ enableCssLayer: true }}>
                <ThemeProvider theme={theme}>
                    <RTL direction={activeDir}>
                        <CssBaseline />
                        <LanguageCurrencyProvider>
                            {children}
                        </LanguageCurrencyProvider>
                    </RTL>
                </ThemeProvider>
            </AppRouterCacheProvider>
        </>
    );
};

export default MyApp;
