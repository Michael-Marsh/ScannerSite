  function go1()
    {
	    box = document.forms[0].machine_name;
	    destination = box.options[box.selectedIndex].value;
	    if (destination)  {
	        if (destination.substring(0,7) == "http://") {
	            open(destination,'newone');
	        }
	        else {
	            location.href = destination;
	        }
	    }
    }
      function go2()
    {
	    box = document.forms[0].industry_name;
	    destination = box.options[box.selectedIndex].value;
	    if (destination)  {
	        if (destination.substring(0,7) == "http://") {
	            open(destination,'newone');
	        }
	        else {
	            location.href = destination;
	        }
	    }
    }

