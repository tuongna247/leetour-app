import { useState } from "react";
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Divider from '@mui/material/Divider';
import { Grid } from '@mui/material';
import Menu from '@mui/material/Menu';
import Typography from '@mui/material/Typography';
import Link from "next/link";
import { IconChevronDown, IconHelp } from "@tabler/icons-react";
import AppLinks from "./AppLinks";
import QuickLinks from "./QuickLinks";

const AppDD = () => {
  const [anchorEl2, setAnchorEl2] = useState(null);

  const handleClick2 = (event) => {
    setAnchorEl2(event.currentTarget);
  };

  const handleClose2 = () => {
    setAnchorEl2(null);
  };

  return (<>
    <Button
      color="inherit"
      sx={{ color: (theme) => theme.palette.text.secondary }}
      variant="text"
      href="/tours"
      component={Link}
    >
      Tours
    </Button>
    <Box>
      <Button
        aria-label="tour management menu"
        color="inherit"
        variant="text"
        aria-controls="tour-menu"
        aria-haspopup="true"
        sx={{
          bgcolor: anchorEl2 ? "primary.light" : "",
          color: anchorEl2
            ? "primary.main"
            : (theme) => theme.palette.text.secondary,
        }}
        onClick={handleClick2}
        endIcon={
          <IconChevronDown
            size="15"
            style={{ marginLeft: "-5px", marginTop: "2px" }}
          />
        }
      >
        Tour Management
      </Button>
      <Menu
        id="tour-menu"
        anchorEl={anchorEl2}
        keepMounted
        open={Boolean(anchorEl2)}
        onClose={handleClose2}
        anchorOrigin={{ horizontal: "left", vertical: "bottom" }}
        transformOrigin={{ horizontal: "left", vertical: "top" }}
      >
        <Box p={2}>
          <Link href="/admin/tours" style={{ textDecoration: 'none' }}>
            <Button fullWidth sx={{ justifyContent: 'flex-start', mb: 1 }}>
              All Tours
            </Button>
          </Link>
          <Link href="/admin/tours/new" style={{ textDecoration: 'none' }}>
            <Button fullWidth sx={{ justifyContent: 'flex-start' }}>
              Add New Tour
            </Button>
          </Link>
        </Box>
      </Menu>
    </Box>
  </>);
};

export default AppDD;
