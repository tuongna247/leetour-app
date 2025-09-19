'use client'
import { useState } from 'react';
import { useRouter } from 'next/navigation';
import CustomSocialButton from "@/app/components/forms/theme-elements/CustomSocialButton";
import { Stack } from "@mui/system";
import Avatar from '@mui/material/Avatar';
import Box from '@mui/material/Box';
import { Alert, CircularProgress } from '@mui/material';
import { useAuth } from "@/contexts/AuthContext";

const AuthSocialButtons = ({ title }) => {
  const [loading, setLoading] = useState({ google: false, facebook: false });
  const [error, setError] = useState(null);
  const { login } = useAuth();
  const router = useRouter();

  const handleGoogleLogin = async () => {
    setLoading({ ...loading, google: true });
    setError(null);
    
    try {
      // For demo purposes, use test Google account
      // In a real app, this would integrate with Google OAuth
      const result = await login('admin', 'admin123');
      
      if (result.success) {
        router.push('/');
      } else {
        setError('Google login failed. Please try manual login.');
      }
    } catch (error) {
      setError('Google login failed. Please try manual login.');
    } finally {
      setLoading({ ...loading, google: false });
    }
  };

  const handleFacebookLogin = async () => {
    setLoading({ ...loading, facebook: true });
    setError(null);
    
    try {
      // For demo purposes, use test Facebook account
      // In a real app, this would integrate with Facebook OAuth
      const result = await login('user', 'user123');
      
      if (result.success) {
        router.push('/');
      } else {
        setError('Facebook login failed. Please try manual login.');
      }
    } catch (error) {
      setError('Facebook login failed. Please try manual login.');
    } finally {
      setLoading({ ...loading, facebook: false });
    }
  };

  return (
    <>
      {error && (
        <Alert severity="info" sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}
      
      <Stack direction="row" justifyContent="center" spacing={2} mt={3}>
        <CustomSocialButton onClick={handleGoogleLogin} disabled={loading.google || loading.facebook}>
          {loading.google ? (
            <CircularProgress size={16} sx={{ mr: 1 }} />
          ) : (
            <Avatar
              src={"/images/svgs/google-icon.svg"}
              alt={"Google"}
              sx={{
                width: 16,
                height: 16,
                borderRadius: 0,
                mr: 1,
              }}
            />
          )}
          <Box
            sx={{
              display: { xs: "none", sm: "flex" },
              whiteSpace: "nowrap",
              mr: { sm: "3px" },
            }}
          >
            {title}{" "}
          </Box>
          Google
        </CustomSocialButton>
        
        <CustomSocialButton onClick={handleFacebookLogin} disabled={loading.google || loading.facebook}>
          {loading.facebook ? (
            <CircularProgress size={16} sx={{ mr: 1 }} />
          ) : (
            <Avatar
              src={"/images/svgs/facebook-icon.svg"}
              alt={"Facebook"}
              sx={{
                width: 25,
                height: 25,
                borderRadius: 0,
                mr: 1,
              }}
            />
          )}
          <Box
            sx={{
              display: { xs: "none", sm: "flex" },
              whiteSpace: "nowrap",
              mr: { sm: "3px" },
            }}
          >
            {title}{" "}
          </Box>
          FB
        </CustomSocialButton>
      </Stack>
    </>
  );
};

export default AuthSocialButtons;
