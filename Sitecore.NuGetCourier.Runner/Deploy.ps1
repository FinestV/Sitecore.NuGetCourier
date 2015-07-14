if (Test-Path "Deletes.txt") {
    Get-Content "Deletes.txt" | ForEach-Object {
        "Attempting to delete " + $_ 
        if (Test-Path $_) {
            Remove-Item $_ -recurse -force
            $_ + " deleted"
	    }
        else {
	        $_ + " already removed"
        }
    }
    Remove-Item "Deletes.txt"
}