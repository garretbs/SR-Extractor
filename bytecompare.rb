class Image_Parser
	def initialize(images)
		@images = images
	end
	
	def get_everything
		get_ico_signature
		get_num_images
		get_widths
		get_heights
		get_color_counts
		get_zeroes
		get_planes
		get_bit_counts
		get_bytes
		get_offsets
	end
	
	def get_ico_signature
		n = 0
		@images.each do |i|
			puts sprintf("Image %i signature: %s", n+=1, i.read(4).bytes.to_s)
		end
		puts ""
	end
	
	def get_num_images
		n = 0
		@images.each do |i|
			puts sprintf("Image %i num images: %s", n+=1, i.read(2).unpack("S1")[0])
		end
		puts ""
	end
	
	def get_widths
		n = 0
		@images.each do |i|
			puts sprintf("Image %i width: %i", n+=1, i.read(1).unpack("C1")[0])
		end
		puts ""
	end
	
	def get_heights
		n = 0
		@images.each do |i|
			puts sprintf("Image %i height: %i", n+=1, i.read(1).unpack("C1")[0])
		end
		puts ""
	end
	
	def get_color_counts
		n = 0
		@images.each do |i|
			puts sprintf("Image %i colorCount: %i", n+=1, i.read(1).unpack("C1")[0])
		end
		puts ""
	end
	
	def get_zeroes
		n = 0
		@images.each do |i|
			puts sprintf("Image %i reserved (should be 0): %i", n+=1, i.read(1).unpack("C1")[0])
		end
		puts ""
	end
	
	def get_planes
		n = 0
		@images.each do |i|
			puts sprintf("Image %i planes: %i", n+=1, i.read(2).unpack("S1")[0])
		end
		puts ""
	end
	
	def get_bit_counts
		n = 0
		@images.each do |i|
			puts sprintf("Image %i bitCount: %i", n+=1, i.read(2).unpack("S1")[0])
		end
		puts ""
	end
	
	def get_bytes
		n = 0
		@images.each do |i|
			puts sprintf("Image %i bytes: %i", n+=1, i.read(4).unpack("L1")[0])
		end
		puts ""
	end
	
	def get_offsets
		n = 0
		@images.each do |i|
			puts sprintf("Image %i offset (should be 22): %i", n+=1, i.read(4).unpack("L1")[0])
		end
		puts ""
	end
	
	def get_headers
		n = 0
		@images.each do |i|
			i.read(22)
			a = i.read(8).unpack("C8")
			string = ""
			a.each{|u| string <<= u.to_s(16) + " "}
			puts sprintf("Image %i header: %s", n+=1, string)
		end
		puts ""
	end
	
	def get_bmp_info
		n = 0
		@images.each do |i|
			i.read(22)
			#puts sprintf("Image %i BMP signature: %s", n, i.read(2).unpack("S1").to_s)
			#puts sprintf("Image %i BMP bytes: %i", n, i.read(4).unpack("L1")[0])
			#puts sprintf("Image %i BMP 0: %i", n, i.read(2).unpack("S1")[0])
			#puts sprintf("Image %i BMP 0-2: %i", n, i.read(2).unpack("S1")[0])
			#puts sprintf("Image %i BMP offset: %i", n, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i BMP 40: %i", n+=1, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i width: %i", n, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i height: %i", n, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i planes: %i", n, i.read(2).unpack("S1")[0])
			puts sprintf("Image %i bits per pixel: %i", n, i.read(2).unpack("S1")[0])
			puts sprintf("Image %i compression: %i", n, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i bytes (incl. padding): %i", n, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i horiz. ppm: %i", n, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i vert. ppm: %i", n, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i colors in image: %i", n, i.read(4).unpack("L1")[0])
			puts sprintf("Image %i important colors in image: %i", n, i.read(4).unpack("L1")[0])
			puts ""
		end
		puts ""
	end
end

f1 = File.open("z_static.ico", "rb")
f2 = File.open("arms.ico", "rb")
f3 = File.open("1389065903886.ico", "rb")
f4 = File.open("1101.ico", "rb")
images = [f1, f2, f3, f4]

r1 = File.open("ic_ABEAST16.IA", "rb")
#r2 = File.open("ic_ALTEREDB.IA", "rb")
r2 = File.open("ic_SONIC2.IA", "rb")
puts r1.read(10).unpack("C10").to_s
puts r2.read(10).unpack("C10").to_s
puts ""
puts r1.read(8).unpack("C8").to_s
puts r2.read(8).unpack("C8").to_s
puts ""
puts r1.read(8).unpack("C8").to_s
puts r2.read(8).unpack("C8").to_s
puts ""
puts r1.read(8).unpack("C8").to_s
puts r2.read(8).unpack("C8").to_s
r1.close
r2.close
# p = Image_Parser.new(images)
#p.get_everything
# p.get_headers
# p.get_bmp_info

=begin
size = 38584
22.times do |t|
	f1a = File.open("z_static"+t.to_s+".ico", "wb")
	# f1a.write([0, 0, 1, 0].pack("C4")) #signature
	# f1a.write([1].pack("S1")) #num images
	# f1a.write([0].pack("C1")) #width (0 for 256)
	# f1a.write([0].pack("C1")) #height (0 for 256)
	# f1a.write([0].pack("C1")) #color count (0 works)
	# f1a.write([0].pack("C1")) #reserved (always 0)
	# f1a.write([1].pack("S1")) #planes (should be 1)
	# f1a.write([0].pack("S1")) #bitCount
	# f1a.write([size-t].pack("L1")) #bytes (minus the 22 bytes in the header)
	# f1a.write([22].pack("L1")) #offset (should be 22)
	f1.seek(0, IO::SEEK_SET)
	f1.read(22-t)
	f1a.write(f1.read(size-t))
	f1a.close
end
=end

=begin
size = 38584 #actual filesize and filesize in .SR archive
f1a = File.open("z_static2.ico", "wb")
f1a.write([0, 0, 1, 0].pack("C4")) #signature
f1a.write([1].pack("S1")) #num images
f1a.write([0].pack("C1")) #width (0 for 256)
f1a.write([0].pack("C1")) #height (0 for 256)
f1a.write([0].pack("C1")) #color count (0 works)
f1a.write([0].pack("C1")) #reserved (always 0)
f1a.write([1].pack("S1")) #planes (should be 1)
f1a.write([0].pack("S1")) #bitCount
f1a.write([size].pack("L1")) #bytes (minus the 22 bytes in the header)
f1a.write([22].pack("L1")) #offset (should be 22)
#now read everything from f1 and write it into f1a
to_read = 13
f1.read(to_read)
f1a.write(f1.read(size-to_read))
f1a.close
=end


#https://msdn.microsoft.com/en-us/library/ms997538.aspx
#https://blogs.msdn.microsoft.com/oldnewthing/20101018-00/?p=12513/
#width CAN be 0 if it is equal to 256. weird, huh?
#"Originally, the supported range was 1 through 255, but starting in Windows 95 (and Windows NT 4), the value 0 is accepted as representing a width or height of 256. "
#"In practice, a lot of people get lazy about filling in the bColorCount and set it to zero, even for 4-color or 16-color icons."




# im = images[0]
# i = 0
# im.read(2)
# while (i += 1) < 15
	# puts im.read(4).unpack("L1")[0] #fourth one should be byte size
# end


images.each{|f| f.close}